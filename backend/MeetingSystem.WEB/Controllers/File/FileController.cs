using Microsoft.AspNetCore.Mvc;
using MeetingSystem.Common.Utils;
using MeetingSystem.Model.Dto;
using StackExchange.Redis;

namespace MeetingSystem.WEB.Controllers.File
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FileController : ControllerBase
    {
        private readonly StackExchange.Redis.IDatabase db;
        private static readonly string UploadDir = Path.Combine(Path.GetTempPath(), "uploads");
        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt", ".zip", ".rar", ".jpg", ".jpeg", ".png"
        };

        public FileController(IConnectionMultiplexer redis)
        {
            db = redis.GetDatabase();
            Directory.CreateDirectory(UploadDir);
        }

        private bool IsPathSafe(string path)
        {
            var fullPath = Path.GetFullPath(path);
            return fullPath.StartsWith(Path.GetFullPath(UploadDir), StringComparison.OrdinalIgnoreCase);
        }

        private bool IsAllowedExtension(string filename)
        {
            var ext = Path.GetExtension(filename);
            return AllowedExtensions.Contains(ext);
        }

        [HttpPost]
        public async Task<R> QueryChunksTable([FromForm] string hash, [FromForm] int size)
        {
            if (size <= 0) return new R().Error().SetMessage("Invalid file size");

            var fileDto = new FileDto();
            string key = hash;
            if (db.KeyExists(key))
            {
                if (db.KeyType(key) == RedisType.String)
                {
                    fileDto.FileUrl = db.StringGet(key).ToString();
                    fileDto.ChunksTable = Array.Empty<int>();
                }
                else
                {
                    fileDto.ChunksTable = db.ListRange(key, 0, -1)
                        .Select(x => int.TryParse(x.ToString(), out var value) ? value : -1)
                        .Where(x => x >= 0)
                        .ToArray();
                }
            }
            else
            {
                long chunkSize = 1024 * 1024;
                int chunkCount = (int)Math.Ceiling((double)size / chunkSize);
                var table = Enumerable.Range(0, chunkCount).ToArray();
                fileDto.ChunksTable = table;
                var numbersAsString = table.Select(x => (RedisValue)x.ToString()).ToArray();
                foreach (var number in numbersAsString)
                {
                    db.ListRightPush(key, number);
                }
                db.KeyExpire(key, TimeSpan.FromMinutes(60 * 24));
            }
            return new R().OK().SetData(fileDto);
        }

        [HttpPost]
        [RequestSizeLimit(100 * 1024 * 1024)] // 100MB limit per chunk
        public async Task<R> Upload([FromForm] string hash, [FromForm] IFormFile file, [FromForm] int index, [FromForm] int totalChunks, [FromForm] int id, [FromForm] string filename)
        {
            if (!IsAllowedExtension(filename))
                return new R().Error().SetMessage("File type not allowed");

            // Sanitize filename
            var safeName = Path.GetFileName(filename);
            FileDto fileDto = new FileDto();
            string key = hash;

            var chunkPath = Path.Combine(UploadDir, $"{hash}_{index}");
            using (var stream = System.IO.File.Create(chunkPath))
            {
                await file.CopyToAsync(stream);
            }

            db.ListRemove(key, index);
            var chunksTable = db.ListRange(key, 0, -1)
                .Select(x => int.TryParse(x.ToString(), out var value) ? value : -1)
                .Where(x => x >= 0)
                .ToArray();

            fileDto.ChunksTable = chunksTable;
            if (chunksTable.Length == 0)
            {
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
                        System.IO.File.Delete(cp);
                    }
                }
                await db.KeyDeleteAsync(key);
                db.StringSet(key, outputFilePath, TimeSpan.FromMinutes(60 * 24));
                fileDto.FileUrl = outputFilePath;
                return new R().OK().SetData(fileDto).SetMessage("文件上传成功");
            }

            return new R().OK().SetData(fileDto).SetMessage("");
        }

        [HttpPost]
        public R ChunkCount([FromForm] string fileUrl)
        {
            if (!IsPathSafe(fileUrl))
                return new R().Error().SetMessage("Invalid file path");

            if (!System.IO.File.Exists(fileUrl))
                return new R().Error().SetMessage("FILE not found");

            using (var stream = System.IO.File.OpenRead(fileUrl))
            {
                long chunkSize = 1024 * 1024;
                int chunkCount = (int)Math.Ceiling((double)stream.Length / chunkSize);
                return new R().OK().SetData(chunkCount);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DownloadFile([FromForm] string fileUrl, [FromForm] int i, [FromForm] int totalChunks)
        {
            if (!IsPathSafe(fileUrl))
                return BadRequest("Invalid file path");

            if (!System.IO.File.Exists(fileUrl))
                return NotFound();

            using var stream = new FileStream(fileUrl, FileMode.Open);
            long chunkSize = 1024 * 1024;
            long startBytes = i * chunkSize;
            byte[] buffer = (i == totalChunks - 1)
                ? new byte[stream.Length - startBytes]
                : new byte[chunkSize];

            stream.Position = startBytes;
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

            var filename = Path.GetFileName(fileUrl);
            return File(buffer, "application/octet-stream", filename);
        }
    }
}
