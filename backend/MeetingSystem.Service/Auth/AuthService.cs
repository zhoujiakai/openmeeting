using MeetingSystem.DBFactory.Database;
using MeetingSystem.IService.Auth;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.Auth;
using MeetingSystem.Service.Base;
using StackExchange.Redis;

namespace MeetingSystem.Service.Auth
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IDatabase _redisDb;
        public AuthService(MeetingSystemDbContext dbContext, IConnectionMultiplexer redis) : base(dbContext)
        {
            _redisDb = redis.GetDatabase();
        }

        public LoginDto Login(string userName, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                return new LoginDto { Code = 1 };

            string token = Guid.NewGuid().ToString();
            string key = $"auth:{userName}";
            _redisDb.StringSet(key, token, TimeSpan.FromDays(7));

            return new LoginDto { Token = token, UserName = userName, RoleName = user.RoleName };
        }

        public int Logout(string userName)
        {
            _redisDb.KeyDelete($"auth:{userName}");
            return 0;
        }

        public int AddRoles(Roles role)
        {
            role.Status = 1;
            _dbContext.Add(role);
            _dbContext.SaveChanges();
            return 1;
        }

        public int DeleteRoles(int id)
        {
            var role = _dbContext.Roles.Find(id);
            if (role == null) return 0;
            _dbContext.Remove(role);
            _dbContext.SaveChanges();
            return 1;
        }

        public int UpdateRoles(Roles role)
        {
            _dbContext.Update(role);
            _dbContext.SaveChanges();
            return 1;
        }

        public Roles GetRoles(int id)
        {
            return _dbContext.Roles.First(a => a.Id == id);
        }

        public List<Roles> GetRoles()
        {
            return _dbContext.Roles.Where(a => a.RoleName != "管理员").ToList();
        }

        public PageDto GetRolesPage(PageParams page)
        {
            var list = this.GetRoles();
            return new PageDto().SetList(list);
        }

        public int AddUsers(Users user)
        {
            user.Status = 1;
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return 0;
        }

        public int DeleteUsers(int id)
        {
            var user = _dbContext.Users.Find(id);
            if (user == null) return 0;
            _dbContext.Remove(user);
            _dbContext.SaveChanges();
            return 0;
        }

        public int UpdateUsers(Users user)
        {
            _dbContext.Update(user);
            _dbContext.SaveChanges();
            return 0;
        }

        public Users GetUsers(int id)
        {
            return _dbContext.Users.First(a => a.Id == id);
        }

        public List<Users> GetUsers()
        {
            return _dbContext.Users.Where(a => a.RoleName != "管理员").ToList();
        }

        public PageDto GetUsersPage(PageParams page)
        {
            var query = _dbContext.Users
                .Where(a => a.RoleName != "管理员")
                .Where(a => !string.IsNullOrEmpty(page.GroupName) ? a.GroupName == page.GroupName : true)
                .Where(a => !string.IsNullOrEmpty(page.RoleName) ? a.RoleName == page.RoleName : true)
                .Where(a => !string.IsNullOrEmpty(page.UserName) ? a.UserName.Contains(page.UserName) : true);

            var total = query.Count();
            var list = query.Skip((page.Page - 1) * page.Limit).Take(page.Limit).ToList();
            return new PageDto().SetList(list).SetTotal(total);
        }

        public int AddGroups(Groups group)
        {
            group.Status = 1;
            _dbContext.Groups.Add(group);
            _dbContext.SaveChanges();
            return 0;
        }
        public int DeleteGroups(int id)
        {
            var group = _dbContext.Groups.Find(id);
            if (group == null) return 0;
            _dbContext.Remove(group);
            _dbContext.SaveChanges();
            return 0;
        }
        public int UpdateGroups(Groups group)
        {
            _dbContext.Update(group);
            _dbContext.SaveChanges();
            return 0;
        }
        public int UpdateGroupsStatus(Groups group)
        {
            var updatedata = _dbContext.Groups.FirstOrDefault(g => g.Id == group.Id);
            if (updatedata != null)
            {
                updatedata.Status = group.Status;
                _dbContext.Update(updatedata);
                _dbContext.SaveChanges();
            }
            return 0;
        }
        public List<Groups> GetGroups()
        {
            return _dbContext.Groups.ToList();
        }
        public PageDto GetGroupsPage(PageParams page)
        {
            var query = _dbContext.Groups
                .Where(a => !string.IsNullOrEmpty(page.GroupName) ? a.GroupName.Contains(page.GroupName) : true);

            var total = query.Count();
            var list = query.Skip((page.Page - 1) * page.Limit).Take(page.Limit).ToList();
            return new PageDto().SetList(list).SetTotal(total);
        }

        public int ChangePassword(ChangePasswordDto changePassword)
        {
            var user = _dbContext.Users.FirstOrDefault(a => a.UserName == changePassword.UserName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(changePassword.OldPassword, user.Password))
                return 1;

            user.Password = BCrypt.Net.BCrypt.HashPassword(changePassword.NewPassword);
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
            return 0;
        }

        public void SeedData()
        {
            if (_dbContext.Users.Any()) return;

            var roles = new List<Roles>
            {
                new() { RoleName = "管理员", Status = 1 },
                new() { RoleName = "教师", Status = 1 },
                new() { RoleName = "学生", Status = 1 }
            };
            _dbContext.Roles.AddRange(roles);
            _dbContext.SaveChanges();

            var users = new List<Users>
            {
                new() { UserName = "admin", Password = BCrypt.Net.BCrypt.HashPassword("123456"), RoleName = "管理员", GroupName = "系统组", Status = 1 },
                new() { UserName = "teacher", Password = BCrypt.Net.BCrypt.HashPassword("123456"), RoleName = "教师", GroupName = "教研组", Status = 1 },
                new() { UserName = "student", Password = BCrypt.Net.BCrypt.HashPassword("123456"), RoleName = "学生", GroupName = "学习组", Status = 1 }
            };
            _dbContext.Users.AddRange(users);
            _dbContext.SaveChanges();

            var groups = new List<Groups>
            {
                new() { GroupName = "系统组", Status = 1 },
                new() { GroupName = "教研组", Status = 1 },
                new() { GroupName = "学习组", Status = 1 }
            };
            _dbContext.Groups.AddRange(groups);
            _dbContext.SaveChanges();
        }
    }
}
