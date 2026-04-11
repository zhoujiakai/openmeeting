namespace MeetingSystem.Model.Dto
{
    /// <summary>
    /// 登录返回数据类，用于封装登录成功后返回的用户信息
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// 状态码，0表示登录成功
        /// </summary>
        public int Code { get; set; } = 0;

        /// <summary>
        /// 认证令牌（Token）
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string? RoleName { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public LoginDto()
        {
        }
    }
}
