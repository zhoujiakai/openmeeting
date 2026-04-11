namespace MeetingSystem.Model.Dto
{
    public class LoginDto
    {
        public int Code { get; set; } = 0;
        public string? Token { get; set; }
        public string? UserName { get; set; }
        public string? RoleName { get; set; }

        public LoginDto()
        {
        }
    }
}