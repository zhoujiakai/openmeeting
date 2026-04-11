namespace MeetingSystem.Model.Models.MeetingRoom
{
    /// <summary>
    /// 会议室实体类，用于在线会议房间的信息管理
    /// </summary>
    public class MeetingRooms
    {
        /// <summary>
        /// 会议室ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 会议室名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 会议室描述
        /// </summary>
        public string Description { get; set; } = "";

        // 房间中所有人的camera摄像头url
        // public List<string> Cameras { get; set; } = new List<string>();
        // 房间中所有人的microphone麦克风url
        // 在会议过程中一直会变
        // public List<string> Audios { get; set; } = new List<string>();

        /// <summary>
        /// 所有参会人员及其各自的全部URL信息
        /// </summary>
        public List<Participants> ParticipantsList { get; set; } = new List<Participants>();

        /// <summary>
        /// 房间唯一的桌面共享URL（会议过程中会变化）
        /// </summary>
        public string DeskTopShare { get; set; } = "";

        /// <summary>
        /// 房间唯一的消息发送URL（创建会议后不变）
        /// </summary>
        public string Messages { get; set; } = "";

    }
}
