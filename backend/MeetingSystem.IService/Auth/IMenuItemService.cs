using MeetingSystem.IService.Base;
using MeetingSystem.Model.Models.Auth;

namespace MeetingSystem.IService.Auth
{
    /// <summary>
    /// 菜单项服务接口，提供菜单的增删改查及用户菜单权限查询功能
    /// </summary>
    public interface IMenuItemService : IBaseService
    {
        /// <summary>
        /// 获取所有菜单项列表
        /// </summary>
        /// <returns>菜单项集合</returns>
        public IEnumerable<MenuItems> List();

        /// <summary>
        /// 根据ID删除菜单项
        /// </summary>
        /// <param name="id">菜单项ID</param>
        /// <returns>受影响的行数</returns>
        public int Remove(int id);

        /// <summary>
        /// 添加菜单项
        /// </summary>
        /// <param name="menuItem">菜单项信息</param>
        /// <returns>受影响的行数</returns>
        public int Add(MenuItems menuItem);

        /// <summary>
        /// 更新菜单项信息
        /// </summary>
        /// <param name="menuItem">菜单项信息</param>
        /// <returns>受影响的行数</returns>
        public int Update(MenuItems menuItem);

        /// <summary>
        /// 根据角色获取该角色有权访问的菜单列表
        /// </summary>
        /// <param name="role">角色名称</param>
        /// <returns>该角色可访问的菜单项集合</returns>
        public IEnumerable<MenuItems> GetUserMenuList(string role);
    }
}
