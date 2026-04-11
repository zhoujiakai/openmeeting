namespace MeetingSystem.Model.Dto
{
    /// <summary>
    /// 周报数据传输类，用于周报信息的展示和传输
    /// </summary>
    public class WeeklyReportsDto
    {
        /// <summary>
        /// 周报ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 周报主题
        /// </summary>
        public string Theme { get; set; } = "";

        /// <summary>
        /// 所属小组名称
        /// </summary>
        public string GroupName { get; set; } = "";

        /// <summary>
        /// 提交周报的用户名
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 上传日期
        /// </summary>
        public DateTime UploadDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 工作内容描述
        /// </summary>
        public string WorkContent { get; set; } = "";

        /// <summary>
        /// 工作进度安排
        /// </summary>
        public string WorkSchedule { get; set; } = "";

        /// <summary>
        /// 遇到的问题及解决方案
        /// </summary>
        public string ProblemAndSolution { get; set; } = "";

        /// <summary>
        /// 周报附件文件地址
        /// </summary>
        public string FileUrl { get; set; } = "";

        /// <summary>
        /// 下载次数
        /// </summary>
        public int DownloadCount { get; set; }
    }
}
