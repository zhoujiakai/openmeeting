using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MeetingSystem.Model.Models.Auth
{
    [Table("menu_items")]
    public class MenuItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ParentId { get; set; } = -1;
        public string MenuName { get; set; } = "";
        public string MenuUrl { get; set; } = "";
        public string ButtonName { get; set; } = ""; // 为空就是一二级菜单，有值就是按钮
        public int MenuType { get; set; }  // 0：一级菜单，1：二级菜单，2：按钮
        public string MenuIcon { get; set; } = "";
        public int Sort { get; set; }
        public int Iframe { get; set; }
        public string RoleName { get; set; } = "管理员";
        [NotMapped]
        public string ToPath { get; set; } = "";
    }
}
