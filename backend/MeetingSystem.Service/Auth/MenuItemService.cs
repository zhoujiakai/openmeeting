using MeetingSystem.DBFactory.Database;
using MeetingSystem.IService.Auth;
using MeetingSystem.Model.Models.Auth;
using MeetingSystem.Service.Base;
using Microsoft.EntityFrameworkCore;

namespace MeetingSystem.Service.Auth
{
    public class MenuItemService : BaseService, IMenuItemService
    {
        public MenuItemService(MeetingSystemDbContext dbContext) : base(dbContext) { }

        public int Add(MenuItems menuItem)
        {
            _dbContext.Add(menuItem);
            _dbContext.SaveChanges();
            return 1;

        }

        public int Remove(int id)
        {
            var entity = _dbContext.MenuItems.Find(id);
            if (entity == null) return 0;
            _dbContext.MenuItems.Remove(entity);
            return _dbContext.SaveChanges() > 0 ? 1 : 0;
        }

        public IEnumerable<MenuItems> List()
        {
            return _dbContext.Set<MenuItems>().ToList();
        }
        public IEnumerable<MenuItems> GetUserMenuList(string roleName)
        {
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
            return _dbContext.MenuItems.Where(a => roles.GetValueOrDefault(roleName, new string[0] ).Contains(a.RoleName)).ToList();
        }

        public int Update(MenuItems menuItem)
        {
            _dbContext.Update(menuItem);
            _dbContext.SaveChanges();
            return 1;
        }
    }
}
