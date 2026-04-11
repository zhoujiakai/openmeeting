namespace MeetingSystem.Model.Dto
{
    /// <summary>
    /// 文件信息类，用于记录分片上传文件的相关信息
    /// </summary>
    public class FileDto
    {
        /// <summary>
        /// 文件访问地址
        /// </summary>
        public string FileUrl { get; set; } = string.Empty;

        /// <summary>
        /// 分片上传的分片状态表，记录每个分片的上传状态
        /// </summary>
        public int[] ChunksTable { get; set; } = new int[0];
    }
}
