namespace MeetingSystem.Model.Dto
{
    public class FileDto
    {
        public string FileUrl { get; set; } = string.Empty;
        public int[] ChunksTable { get; set; } = new int[0];
    }
}
