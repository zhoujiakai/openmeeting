namespace MeetingSystem.Model.Dto
{
    public class PageParams
    {
        public int Limit { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }
        public string RoleName { get; set; } = "";
        public string GroupName { get; set; } = "";
        public string UserName { get; set; } = "";
        public string WeeklyReportTheme { get; set; } = "";
        public List<DateTime>? MeetingDateTimeRange { get; set; }
        public string MeetingKeywords { get; set; } = "";
        public int MeetingId { get; set; } = 0;
    }
}
