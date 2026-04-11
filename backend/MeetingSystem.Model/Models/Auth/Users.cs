using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingSystem.Model.Models.Auth
{
    /// <summary>
    /// 用户实体类，对应数据库users表，存储用户账户信息
    /// </summary>
    [Table("users")]
    public class Users
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Users(){}

        /// <summary>
        /// 带ID的构造函数
        /// </summary>
        /// <param name="id">用户ID</param>
        public Users(int id)
        {
            Id = id;
        }

        /// <summary>
        /// 用户ID，主键，自增
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = "";

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; } = "";

        /// <summary>
        /// 所属小组名称
        /// </summary>
        public string GroupName { get; set; } = "";

        /// <summary>
        /// 用户状态：0-不启用，1-启用
        /// </summary>
        public int Status { get; set; } = 1;

        /// <summary>
        /// 用户头像地址
        /// </summary>
        public string Image { get; set; } = "https://meeting.oss-cn-qingdao.aliyuncs.com/default.png";
    }
}
