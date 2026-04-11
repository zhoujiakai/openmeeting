namespace MeetingSystem.Model.Models.MeetingRoom
{
    /// <summary>
    /// 参会者实体类，存储会议中每个参与者的信息和媒体流地址
    /// </summary>
    public class Participants
    {
        /// <summary>
        /// 参会者用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 参会者用户名
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 参会者头像地址
        /// </summary>
        public string Image { get; set; } = "";

        /// <summary>
        /// 摄像头媒体流地址
        /// </summary>
        public string Camera { get; set; } = "";

        /// <summary>
        /// 麦克风媒体流地址
        /// </summary>
        public string Audio { get; set; } = "";

        // public string DesktopShare { get; set; } = "";
    }
}
