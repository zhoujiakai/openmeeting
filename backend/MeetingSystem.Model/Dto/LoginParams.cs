namespace MeetingSystem.Model.Dto
{
    /// <summary>
    /// 登录请求参数类，用于接收用户登录时提交的参数
    /// </summary>
    public class LoginParams
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; } = "";
    }
}
