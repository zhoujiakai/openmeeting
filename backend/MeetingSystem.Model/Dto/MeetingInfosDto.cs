namespace MeetingSystem.Model.Dto
{
    public class MeetingInfosDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Keywords { get; set; } = "";
        public string GroupName { get; set; } = "";
        public string UserName { get; set; } = "";
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Place { get; set; } = "";
        public string FileUrl { get; set; } = "";
        public int MeetingType { get; set; }  // 0：学术会议，1：线下会议，2：线上会议
        public string Code { get; set; } = "";  // 如果是线上会议有会议编码
    }
}
