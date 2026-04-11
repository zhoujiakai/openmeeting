using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingSystem.Model.Models.Auth
{
    [Table("users")]
    public class Users
    {
        public Users(){}
        public Users(int id)
        {
            Id = id;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string RoleName { get; set; } = "";
        public string GroupName { get; set; } = "";
        public int Status { get; set; } = 1;  // 0:不启用；1：启用
        public string Image { get; set; } = "https://meeting.oss-cn-qingdao.aliyuncs.com/default.png";
    }
}
