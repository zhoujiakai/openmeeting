namespace MeetingSystem.Model.Dto
{
    /// <summary>
    /// 修改密码参数类，用于接收用户修改密码时提交的数据
    /// </summary>
    public class ChangePasswordDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 原密码
        /// </summary>
        public string OldPassword { get; set; } = "";

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; } = "";
    }
}
