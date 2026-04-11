namespace MeetingSystem.Model.Dto
{
    /// <summary>
    /// 会议报告数据传输类，用于会议报告的展示和传输
    /// </summary>
    public class MeetingReportsDto
    {
        /// <summary>
        /// 报告ID
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// 报告标题
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// 对应的会议标题
        /// </summary>
        public string MeetingTitle { get; set; } = "";

        /// <summary>
        /// 对应的会议ID
        /// </summary>
        public int MeetingId { get; set; } = 0;

        /// <summary>
        /// 所属小组名称
        /// </summary>
        public string GroupName { get; set; } = "";

        /// <summary>
        /// 报告附件文件地址
        /// </summary>
        public string FileUrl { get; set; } = "";

        /// <summary>
        /// 上传报告的用户名
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 上传日期
        /// </summary>
        public DateTime UploadDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 下载次数
        /// </summary>
        public int DownloadCount { get; set; } = 0;

        /// <summary>
        /// 报告状态
        /// </summary>
        public int Status { get; set; } = 1;

    }
}
