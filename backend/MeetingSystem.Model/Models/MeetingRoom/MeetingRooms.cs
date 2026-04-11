namespace MeetingSystem.Model.Models.MeetingRoom
{
    public class MeetingRooms
    {

        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        // 房间中所有人的camera摄像头url
        // public List<string> Cameras { get; set; } = new List<string>();
        // 房间中所有人的microphone麦克风url
        // 在会议过程中一直会变
        // public List<string> Audios { get; set; } = new List<string>();

        // 所有参会的人和他们各自的全部url
        public List<Participants> ParticipantsList { get; set; } = new List<Participants>();
        // 房间的唯一desktopshare共享桌面url
        // 在会议过程中一直会变
        public string DeskTopShare { get; set; } = "";
        // 房间的唯一chatMessage发送消息url
        // 创建会议之后是不会变的
        public string Messages { get; set; } = "";

    }
}
