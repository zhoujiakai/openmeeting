namespace MeetingSystem.Model.Dto
{
    /// <summary>
    /// 菜单查询参数类，用于根据角色名称获取对应的菜单项
    /// </summary>
    public class MenuParams
    {
        /// <summary>
        /// 角色名称，用于查询该角色对应的菜单权限
        /// </summary>
        public string RoleName { get; set; } = "";
    }
}
