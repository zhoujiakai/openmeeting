namespace MeetingSystem.Model.Dto
{
    public class WeeklyReportsDto
    {
        public int Id { get; set; }
        public string Theme { get; set; } = "";
        public string GroupName { get; set; } = "";
        public string UserName { get; set; } = "";
        public DateTime UploadDate { get; set; } = DateTime.Now;
        public string WorkContent { get; set; } = "";
        public string WorkSchedule { get; set; } = "";
        public string ProblemAndSolution { get; set; } = "";

        public string FileUrl { get; set; } = "";
        public int DownloadCount { get; set; }
    }
}
