using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingSystem.Model.Models.Meeting

{
    [Table("meeting_reports")]
    public class MeetingReports
    {
        public MeetingReports(){}
        public MeetingReports(int id)
        {
            Id = id;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; } = "";
        // 对应的会议ID
        public int MeetingId { get; set; } = 1;
        // 小组名字
        public string GroupName { get; set; } = "";
        // 文件路径
        public string FileUrl { get; set; } = "";

        public string UserName { get; set; } = "";
        public DateTime UploadDate { get; set; } = DateTime.Now;

        public int DownloadCount { get; set; } = 0;

        public int Status { get; set; } = 1;

    }
}