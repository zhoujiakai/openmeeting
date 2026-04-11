using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingSystem.Model.Models.Meeting

{
    /// <summary>
    /// 会议报告实体类，对应数据库meeting_reports表，存储会议报告信息
    /// </summary>
    [Table("meeting_reports")]
    public class MeetingReports
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MeetingReports(){}

        /// <summary>
        /// 带ID的构造函数
        /// </summary>
        /// <param name="id">报告ID</param>
        public MeetingReports(int id)
        {
            Id = id;
        }

        /// <summary>
        /// 报告ID，主键，自增
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 报告标题
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// 对应的会议ID
        /// </summary>
        public int MeetingId { get; set; } = 1;

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
