using MeetingSystem.IService.Base;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.Auth;

namespace MeetingSystem.IService.Auth
{
    public interface IAuthService
    {
        public LoginDto Login(string username, string password);
        public int Logout(string username);

        public int AddRoles(Roles role);
        public int DeleteRoles(int id);
        public int UpdateRoles(Roles role);
        public Roles GetRoles(int id);
        public List<Roles> GetRoles();
        public PageDto GetRolesPage(PageParams page);

        public int AddUsers(Users user);
        public int DeleteUsers(int id);
        public int UpdateUsers(Users user);
        public Users GetUsers(int id);
        public List<Users> GetUsers();
        public PageDto GetUsersPage(PageParams page);

        public int AddGroups(Groups group);
        public int DeleteGroups(int id);
        public int UpdateGroups(Groups group);
        public int UpdateGroupsStatus(Groups group);
        public List<Groups> GetGroups();
        public PageDto GetGroupsPage(PageParams page);

        public int ChangePassword(ChangePasswordDto changePassword);
        public void SeedData();
    }
}
