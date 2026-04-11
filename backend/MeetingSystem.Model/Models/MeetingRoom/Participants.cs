namespace MeetingSystem.Model.Models.MeetingRoom
{
    public class Participants
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public string Image { get; set; } = "";
        public string Camera { get; set; } = "";
        public string Audio { get; set; } = "";
        // public string DesktopShare { get; set; } = "";
    }
}
