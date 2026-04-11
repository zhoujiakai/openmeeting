using Microsoft.AspNetCore.Mvc;
using MeetingSystem.Common.Utils;
using MeetingSystem.Model.Dto;
using StackExchange.Redis;

namespace MeetingSystem.WEB.Controllers.File
{
    /// <summary>
    /// 文件管理控制器，提供大文件分片上传、分片查询、分片下载等功能
    /// 使用Redis记录分片上传进度
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FileController : ControllerBase
    {
        /// <summary>Redis数据库实例，用于存储分片上传状态</summary>
        private readonly StackExchange.Redis.IDatabase db;

        /// <summary>文件上传临时目录，位于系统临时目录下的uploads文件夹</summary>
        private static readonly string UploadDir = Path.Combine(Path.GetTempPath(), "uploads");

        /// <summary>允许上传的文件扩展名白名单</summary>
        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt", ".zip", ".rar", ".jpg", ".jpeg", ".png"
        };

        /// <summary>
        /// 构造函数，初始化Redis连接并创建上传目录
        /// </summary>
        /// <param name="redis">Redis连接复用器</param>
        public FileController(IConnectionMultiplexer redis)
        {
            db = redis.GetDatabase();
            // 确保上传目录存在
            Directory.CreateDirectory(UploadDir);
        }

        /// <summary>
        /// 检查文件路径是否安全（防止目录遍历攻击）
        /// </summary>
        /// <param name="path">待检查的文件路径</param>
        /// <returns>路径是否在上传目录内</returns>
        private bool IsPathSafe(string path)
        {
            var fullPath = Path.GetFullPath(path);
            return fullPath.StartsWith(Path.GetFullPath(UploadDir), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 检查文件扩展名是否在允许的白名单中
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>扩展名是否被允许</returns>
        private bool IsAllowedExtension(string filename)
        {
            var ext = Path.GetExtension(filename);
            return AllowedExtensions.Contains(ext);
        }

        /// <summary>
        /// 查询分片上传进度表，用于断点续传
        /// </summary>
        /// <param name="hash">文件哈希值，作为Redis中的唯一标识</param>
        /// <param name="size">文件总大小（字节）</param>
        /// <returns>返回已上传的分片信息或未上传的分片列表</returns>
        [HttpPost]
        public async Task<R> QueryChunksTable([FromForm] string hash, [FromForm] int size)
        {
            // 校验文件大小是否有效
            if (size <= 0) return new R().Error().SetMessage("Invalid file size");

            var fileDto = new FileDto();
            string key = hash;
            if (db.KeyExists(key))
            {
                if (db.KeyType(key) == RedisType.String)
                {
                    // 文件已完整上传过，直接返回文件路径
                    fileDto.FileUrl = db.StringGet(key).ToString();
                    fileDto.ChunksTable = Array.Empty<int>();
                }
                else
                {
                    // 文件正在上传中，返回剩余未上传的分片索引
                    fileDto.ChunksTable = db.ListRange(key, 0, -1)
                        .Select(x => int.TryParse(x.ToString(), out var value) ? value : -1)
                        .Where(x => x >= 0)
                        .ToArray();
                }
            }
            else
            {
                // 首次上传，计算总分片数并初始化分片表
                long chunkSize = 1024 * 1024; // 每个分片1MB
                int chunkCount = (int)Math.Ceiling((double)size / chunkSize);
                var table = Enumerable.Range(0, chunkCount).ToArray();
                fileDto.ChunksTable = table;
                // 将所有分片索引写入Redis列表
                var numbersAsString = table.Select(x => (RedisValue)x.ToString()).ToArray();
                foreach (var number in numbersAsString)
                {
                    db.ListRightPush(key, number);
                }
                // 设置Redis键过期时间为24小时
                db.KeyExpire(key, TimeSpan.FromMinutes(60 * 24));
            }
            return new R().OK().SetData(fileDto);
        }

        /// <summary>
        /// 上传单个文件分片，当所有分片上传完毕后自动合并为完整文件
        /// </summary>
        /// <param name="hash">文件哈希值</param>
        /// <param name="file">分片文件数据</param>
        /// <param name="index">当前分片索引</param>
        /// <param name="totalChunks">总分片数</param>
        /// <param name="id">关联业务ID</param>
        /// <param name="filename">原始文件名</param>
        /// <returns>上传结果，包含剩余分片表；全部上传完成时返回文件路径</returns>
        [HttpPost]
        [RequestSizeLimit(100 * 1024 * 1024)] // 每个分片最大100MB
        public async Task<R> Upload([FromForm] string hash, [FromForm] IFormFile file, [FromForm] int index, [FromForm] int totalChunks, [FromForm] int id, [FromForm] string filename)
        {
            // 校验文件类型是否允许
            if (!IsAllowedExtension(filename))
                return new R().Error().SetMessage("File type not allowed");

            // 清理文件名，防止路径注入
            var safeName = Path.GetFileName(filename);
            FileDto fileDto = new FileDto();
            string key = hash;

            // 将分片保存到临时文件
            var chunkPath = Path.Combine(UploadDir, $"{hash}_{index}");
            using (var stream = System.IO.File.Create(chunkPath))
            {
                await file.CopyToAsync(stream);
            }

            // 从Redis列表中移除已上传的分片索引
            db.ListRemove(key, index);
            // 获取剩余未上传的分片列表
            var chunksTable = db.ListRange(key, 0, -1)
                .Select(x => int.TryParse(x.ToString(), out var value) ? value : -1)
                .Where(x => x >= 0)
                .ToArray();

            fileDto.ChunksTable = chunksTable;
            // 如果所有分片都已上传完毕，进行合并
            if (chunksTable.Length == 0)
            {
                // 合并所有分片为最终文件
                var outputFilePath = Path.Combine(UploadDir, $"{hash}_{safeName}");
                using (var outputStream = System.IO.File.Create(outputFilePath))
                {
                    for (int i = 0; i < totalChunks; i++)
                    {
                        var cp = Path.Combine(UploadDir, $"{hash}_{i}");
                        using (var inputStream = System.IO.File.OpenRead(cp))
                        {
                            await inputStream.CopyToAsync(outputStream);
                        }
                        // 合并后删除分片临时文件
                        System.IO.File.Delete(cp);
                    }
                }
                // 删除Redis中的分片列表，改为存储完整文件路径
                await db.KeyDeleteAsync(key);
                db.StringSet(key, outputFilePath, TimeSpan.FromMinutes(60 * 24));
                fileDto.FileUrl = outputFilePath;
                return new R().OK().SetData(fileDto).SetMessage("文件上传成功");
            }

            // 还有分片未上传，返回剩余分片表
            return new R().OK().SetData(fileDto).SetMessage("");
        }

        /// <summary>
        /// 获取文件的总分片数，用于下载时的分片计算
        /// </summary>
        /// <param name="fileUrl">文件完整路径</param>
        /// <returns>文件的总分片数</returns>
        [HttpPost]
        public R ChunkCount([FromForm] string fileUrl)
        {
            // 校验路径安全性，防止目录遍历
            if (!IsPathSafe(fileUrl))
                return new R().Error().SetMessage("Invalid file path");

            if (!System.IO.File.Exists(fileUrl))
                return new R().Error().SetMessage("FILE not found");

            using (var stream = System.IO.File.OpenRead(fileUrl))
            {
                long chunkSize = 1024 * 1024; // 每个分片1MB
                // 根据文件大小计算总分片数
                int chunkCount = (int)Math.Ceiling((double)stream.Length / chunkSize);
                return new R().OK().SetData(chunkCount);
            }
        }

        /// <summary>
        /// 分片下载文件，根据指定索引返回对应的文件分片数据
        /// </summary>
        /// <param name="fileUrl">文件完整路径</param>
        /// <param name="i">要下载的分片索引</param>
        /// <param name="totalChunks">总分片数</param>
        /// <returns>指定分片的二进制数据流</returns>
        [HttpPost]
        public async Task<IActionResult> DownloadFile([FromForm] string fileUrl, [FromForm] int i, [FromForm] int totalChunks)
        {
            // 校验路径安全性，防止目录遍历
            if (!IsPathSafe(fileUrl))
                return BadRequest("Invalid file path");

            if (!System.IO.File.Exists(fileUrl))
                return NotFound();

            using var stream = new FileStream(fileUrl, FileMode.Open);
            long chunkSize = 1024 * 1024; // 每个分片1MB
            long startBytes = i * chunkSize;
            // 最后一个分片的大小可能不足1MB，需要特殊处理
            byte[] buffer = (i == totalChunks - 1)
                ? new byte[stream.Length - startBytes]
                : new byte[chunkSize];

            // 定位到分片起始位置并读取数据
            stream.Position = startBytes;
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

            var filename = Path.GetFileName(fileUrl);
            return File(buffer, "application/octet-stream", filename);
        }
    }
}
