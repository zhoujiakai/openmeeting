using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MeetingSystem.Model.Models.Auth
{
    /// <summary>
    /// 菜单项实体类，对应数据库menu_items表，存储系统菜单和按钮权限信息
    /// </summary>
    [Table("menu_items")]
    public class MenuItems
    {
        /// <summary>
        /// 菜单项ID，主键，自增
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 父级菜单ID，-1表示一级菜单
        /// </summary>
        public int ParentId { get; set; } = -1;

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; } = "";

        /// <summary>
        /// 菜单路径URL
        /// </summary>
        public string MenuUrl { get; set; } = "";

        /// <summary>
        /// 按钮名称，为空则是一二级菜单，有值则是按钮
        /// </summary>
        public string ButtonName { get; set; } = "";

        /// <summary>
        /// 菜单类型：0-一级菜单，1-二级菜单，2-按钮
        /// </summary>
        public int MenuType { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string MenuIcon { get; set; } = "";

        /// <summary>
        /// 排序序号
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否为内嵌页面（Iframe）
        /// </summary>
        public int Iframe { get; set; }

        /// <summary>
        /// 关联的角色名称
        /// </summary>
        public string RoleName { get; set; } = "管理员";

        /// <summary>
        /// 转换后的路径（不映射到数据库）
        /// </summary>
        [NotMapped]
        public string ToPath { get; set; } = "";
    }
}
