using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingSystem.Model.Models.WeeklyReport
{
    /// <summary>
    /// 周报实体类，对应数据库weekly_reports表，存储周报信息
    /// </summary>
    [Table("weekly_reports")]
    public class WeeklyReports
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public WeeklyReports()
        {
        }

        /// <summary>
        /// 带ID的构造函数
        /// </summary>
        /// <param name="id">周报ID</param>
        public WeeklyReports(int id)
        {
            Id = id;
        }

        /// <summary>
        /// 周报ID，主键，自增
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public int DownloadCount { get; set; } = 0;

        /// <summary>
        /// 教师评语
        /// </summary>
        public string TeacherComment { get; set; } = "";

        /// <summary>
        /// 审核状态：0-未通过，1-通过
        /// </summary>
        public int Status { get; set; } = 0;

    }
}
