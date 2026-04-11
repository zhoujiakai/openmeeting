using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingSystem.Model.Models.Auth
{
    /// <summary>
    /// 角色实体类，对应数据库roles表，存储角色信息
    /// </summary>
    [Table("roles")]
    public class Roles
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Roles(){}

        /// <summary>
        /// 带ID的构造函数
        /// </summary>
        /// <param name="id">角色ID</param>
        public Roles(int id)
        {
            Id = id;
        }

        /// <summary>
        /// 角色ID，主键，自增
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; } = "";

        /// <summary>
        /// 角色状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 该角色拥有的菜单项列表（不映射到数据库）
        /// </summary>
        [NotMapped]
        public List<MenuItems> MenuItemList { get; set; } = new List<MenuItems>();
    }
}
