namespace MeetingSystem.Model.Dto
{
    /// <summary>
    /// 分页查询参数类，用于接收分页查询时的筛选和分页参数
    /// </summary>
    public class PageParams
    {
        /// <summary>
        /// 每页显示数量
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 角色名称（用于按角色筛选）
        /// </summary>
        public string RoleName { get; set; } = "";

        /// <summary>
        /// 小组名称（用于按小组筛选）
        /// </summary>
        public string GroupName { get; set; } = "";

        /// <summary>
        /// 用户名（用于按用户筛选）
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 周报主题（用于按周报主题筛选）
        /// </summary>
        public string WeeklyReportTheme { get; set; } = "";

        /// <summary>
        /// 会议日期范围（用于按日期范围筛选会议）
        /// </summary>
        public List<DateTime>? MeetingDateTimeRange { get; set; }

        /// <summary>
        /// 会议关键词（用于按关键词模糊搜索会议）
        /// </summary>
        public string MeetingKeywords { get; set; } = "";

        /// <summary>
        /// 会议ID（用于查询指定会议相关数据）
        /// </summary>
        public int MeetingId { get; set; } = 0;
    }
}
