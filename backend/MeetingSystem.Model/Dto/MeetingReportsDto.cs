namespace MeetingSystem.Model.Dto

{
    public class MeetingReportsDto
    {
        public int Id { get; set; } = 0;
        public string Title { get; set; } = "";
        // 对应的会议ID
        public string MeetingTitle { get; set; } = "";
        // 会议ID
        public int MeetingId { get; set; } = 0;
        // 小组名称
        public string GroupName { get; set; } = "";
        // 文件路径
        public string FileUrl { get; set; } = "";

        public string UserName { get; set; } = "";
        public DateTime UploadDate { get; set; } = DateTime.Now;

        public int DownloadCount { get; set; } = 0;

        public int Status { get; set; } = 1;

    }
}
