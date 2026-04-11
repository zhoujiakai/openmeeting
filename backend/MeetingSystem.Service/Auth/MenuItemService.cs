using MeetingSystem.DBFactory.Database;
using MeetingSystem.IService.Auth;
using MeetingSystem.Model.Models.Auth;
using MeetingSystem.Service.Base;
using Microsoft.EntityFrameworkCore;

namespace MeetingSystem.Service.Auth
{
    /// <summary>
    /// 菜单项服务，提供菜单的增删改查以及基于角色的菜单权限过滤功能。
    /// 不同角色可访问的菜单范围由角色层级关系决定。
    /// </summary>
    public class MenuItemService : BaseService, IMenuItemService
    {
        /// <summary>
        /// 构造函数，注入数据库上下文
        /// </summary>
        /// <param name="dbContext">会议系统数据库上下文</param>
        public MenuItemService(MeetingSystemDbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// 添加菜单项
        /// </summary>
        /// <param name="menuItem">菜单项实体对象</param>
        /// <returns>成功返回 1</returns>
        public int Add(MenuItems menuItem)
        {
            _dbContext.Add(menuItem);
            _dbContext.SaveChanges();
            return 1;

        }

        /// <summary>
        /// 根据ID删除菜单项
        /// </summary>
        /// <param name="id">菜单项ID</param>
        /// <returns>成功返回 1，菜单项不存在或删除失败返回 0</returns>
        public int Remove(int id)
        {
            var entity = _dbContext.MenuItems.Find(id);
            if (entity == null) return 0;
            _dbContext.MenuItems.Remove(entity);
            return _dbContext.SaveChanges() > 0 ? 1 : 0;
        }

        /// <summary>
        /// 获取所有菜单项列表
        /// </summary>
        /// <returns>菜单项集合</returns>
        public IEnumerable<MenuItems> List()
        {
            return _dbContext.Set<MenuItems>().ToList();
        }

        /// <summary>
        /// 根据角色名称获取该角色可见的菜单列表。
        /// 通过角色层级关系，上级角色可以看到下级角色的菜单。
        /// </summary>
        /// <param name="roleName">当前用户的角色名称</param>
        /// <returns>该角色可见的菜单项集合</returns>
        public IEnumerable<MenuItems> GetUserMenuList(string roleName)
        {
            // 定义角色层级关系：每个角色可以看到自身及其下级角色的菜单
            var roles = new Dictionary<string, string[]>
            {
                //{ "admin",    ["admin", "teacher", "groupadmin", "student"] },
                //{ "teacher", ["teacher", "groupadmin", "student"] },
                //{ "groupadmin", ["groupadmin", "student"] },
                //{ "student", ["student"] },
                {"管理员", ["管理员", "老师", "小组管理员", "学生"] },
                {"老师", ["老师", "小组管理员", "学生"] },
                {"小组管理员", ["小组管理员", "学生"] },
                {"学生", ["学生"] },
            };

            // 根据当前角色获取其可访问的角色列表，并筛选对应菜单
            return _dbContext.MenuItems.Where(a => roles.GetValueOrDefault(roleName, new string[0] ).Contains(a.RoleName)).ToList();
        }

        /// <summary>
        /// 更新菜单项信息
        /// </summary>
        /// <param name="menuItem">菜单项实体对象</param>
        /// <returns>成功返回 1</returns>
        public int Update(MenuItems menuItem)
        {
            _dbContext.Update(menuItem);
            _dbContext.SaveChanges();
            return 1;
        }
    }
}
