using MeetingSystem.IService.Base;
using MeetingSystem.Model.Models.Auth;

namespace MeetingSystem.IService.Auth
{
    public interface IMenuItemService : IBaseService
    {
        public IEnumerable<MenuItems> List();
        public int Remove(int id);
        public int Add(MenuItems menuItem);
        public int Update(MenuItems menuItem);
        public IEnumerable<MenuItems> GetUserMenuList(string role);
    }
}
