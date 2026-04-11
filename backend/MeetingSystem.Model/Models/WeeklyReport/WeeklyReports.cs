using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingSystem.Model.Models.WeeklyReport
{
    [Table("weekly_reports")]
    public class WeeklyReports
    {
        public WeeklyReports()
        {
        }

        public WeeklyReports(int id)
        {
            Id = id;
        }
 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Theme { get; set; } = "";
        public string GroupName { get; set; } = "";
        public string UserName { get; set; } = "";
        public DateTime UploadDate { get; set; } = DateTime.Now;
        public string WorkContent { get; set; } = "";
        public string WorkSchedule { get; set; } = "";
        public string ProblemAndSolution { get; set; } = "";

        public string FileUrl { get; set; } = "";
        public int DownloadCount { get; set; } = 0;
        public string TeacherComment { get; set; } = "";
        public int Status { get; set; } = 0; // 0: 未通过；1: 通过

    }
}
