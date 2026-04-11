namespace MeetingSystem.Model.Dto
{
    /// <summary>
    /// 会议信息数据传输类，用于会议信息的展示和传输
    /// </summary>
    public class MeetingInfosDto
    {
        /// <summary>
        /// 会议ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 会议标题
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// 会议关键词
        /// </summary>
        public string Keywords { get; set; } = "";

        /// <summary>
        /// 所属小组名称
        /// </summary>
        public string GroupName { get; set; } = "";

        /// <summary>
        /// 创建会议的用户名
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 会议开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 会议结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 会议地点
        /// </summary>
        public string Place { get; set; } = "";

        /// <summary>
        /// 会议附件文件地址
        /// </summary>
        public string FileUrl { get; set; } = "";

        /// <summary>
        /// 会议类型：0-学术会议，1-线下会议，2-线上会议
        /// </summary>
        public int MeetingType { get; set; }

        /// <summary>
        /// 线上会议的会议编码（仅线上会议有值）
        /// </summary>
        public string Code { get; set; } = "";
    }
}
