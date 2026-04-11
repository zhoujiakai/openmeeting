using MeetingSystem.DBFactory.Database;
using MeetingSystem.IService.Auth;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.Auth;
using MeetingSystem.Service.Base;
using StackExchange.Redis;

namespace MeetingSystem.Service.Auth
{
    /// <summary>
    /// 认证与授权服务，提供用户登录登出、角色管理、用户管理、分组管理及密码修改等功能。
    /// 使用 Redis 存储用户登录令牌（Token），支持会话管理。
    /// </summary>
    public class AuthService : BaseService, IAuthService
    {
        /// <summary>
        /// Redis 数据库实例，用于存储和管理用户登录令牌
        /// </summary>
        private readonly IDatabase _redisDb;

        /// <summary>
        /// 构造函数，注入数据库上下文和 Redis 连接
        /// </summary>
        /// <param name="dbContext">会议系统数据库上下文</param>
        /// <param name="redis">Redis 连接多路复用器</param>
        public AuthService(MeetingSystemDbContext dbContext, IConnectionMultiplexer redis) : base(dbContext)
        {
            _redisDb = redis.GetDatabase();
        }

        /// <summary>
        /// 用户登录验证。校验用户名和密码，成功后生成 Token 并存储到 Redis。
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码（明文）</param>
        /// <returns>登录结果 DTO，包含 Token、用户名和角色名；失败时 Code 为 1</returns>
        public LoginDto Login(string userName, string password)
        {
            // 根据用户名查找用户
            var user = _dbContext.Users.FirstOrDefault(u => u.UserName == userName);

            // 验证用户是否存在以及密码是否正确（BCrypt 哈希校验）
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                return new LoginDto { Code = 1 };

            // 生成唯一 Token 作为登录凭证
            string token = Guid.NewGuid().ToString();
            string key = $"auth:{userName}";

            // 将 Token 存入 Redis，设置 7 天过期时间
            _redisDb.StringSet(key, token, TimeSpan.FromDays(7));

            return new LoginDto { Token = token, UserName = userName, RoleName = user.RoleName };
        }

        /// <summary>
        /// 用户登出，从 Redis 中删除对应的登录令牌
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>始终返回 0 表示成功</returns>
        public int Logout(string userName)
        {
            // 删除 Redis 中的登录令牌
            _redisDb.KeyDelete($"auth:{userName}");
            return 0;
        }

        /// <summary>
        /// 添加新角色
        /// </summary>
        /// <param name="role">角色实体对象</param>
        /// <returns>成功返回 1</returns>
        public int AddRoles(Roles role)
        {
            role.Status = 1; // 设置角色状态为启用
            _dbContext.Add(role);
            _dbContext.SaveChanges();
            return 1;
        }

        /// <summary>
        /// 根据ID删除角色
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>成功返回 1，角色不存在返回 0</returns>
        public int DeleteRoles(int id)
        {
            var role = _dbContext.Roles.Find(id);
            if (role == null) return 0;
            _dbContext.Remove(role);
            _dbContext.SaveChanges();
            return 1;
        }

        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="role">角色实体对象</param>
        /// <returns>成功返回 1</returns>
        public int UpdateRoles(Roles role)
        {
            _dbContext.Update(role);
            _dbContext.SaveChanges();
            return 1;
        }

        /// <summary>
        /// 根据ID获取单个角色
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>角色实体对象</returns>
        public Roles GetRoles(int id)
        {
            return _dbContext.Roles.First(a => a.Id == id);
        }

        /// <summary>
        /// 获取所有角色列表（排除"管理员"角色）
        /// </summary>
        /// <returns>角色列表</returns>
        public List<Roles> GetRoles()
        {
            return _dbContext.Roles.Where(a => a.RoleName != "管理员").ToList();
        }

        /// <summary>
        /// 获取角色分页列表
        /// </summary>
        /// <param name="page">分页参数</param>
        /// <returns>分页结果 DTO</returns>
        public PageDto GetRolesPage(PageParams page)
        {
            var list = this.GetRoles();
            return new PageDto().SetList(list);
        }

        /// <summary>
        /// 添加新用户。自动对密码进行 BCrypt 哈希加密。
        /// </summary>
        /// <param name="user">用户实体对象（密码为明文）</param>
        /// <returns>始终返回 0</returns>
        public int AddUsers(Users user)
        {
            user.Status = 1; // 设置用户状态为启用
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); // 对密码进行哈希加密
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return 0;
        }

        /// <summary>
        /// 根据ID删除用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>始终返回 0，用户不存在也返回 0</returns>
        public int DeleteUsers(int id)
        {
            var user = _dbContext.Users.Find(id);
            if (user == null) return 0;
            _dbContext.Remove(user);
            _dbContext.SaveChanges();
            return 0;
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user">用户实体对象</param>
        /// <returns>始终返回 0</returns>
        public int UpdateUsers(Users user)
        {
            _dbContext.Update(user);
            _dbContext.SaveChanges();
            return 0;
        }

        /// <summary>
        /// 根据ID获取单个用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>用户实体对象</returns>
        public Users GetUsers(int id)
        {
            return _dbContext.Users.First(a => a.Id == id);
        }

        /// <summary>
        /// 获取所有用户列表（排除"管理员"用户）
        /// </summary>
        /// <returns>用户列表</returns>
        public List<Users> GetUsers()
        {
            return _dbContext.Users.Where(a => a.RoleName != "管理员").ToList();
        }

        /// <summary>
        /// 获取用户分页列表，支持按分组、角色和用户名进行筛选
        /// </summary>
        /// <param name="page">分页参数，包含筛选条件和分页信息</param>
        /// <returns>包含用户列表和总数的分页结果 DTO</returns>
        public PageDto GetUsersPage(PageParams page)
        {
            // 构建查询条件：排除管理员，并根据传入的筛选参数动态过滤
            var query = _dbContext.Users
                .Where(a => a.RoleName != "管理员")
                .Where(a => !string.IsNullOrEmpty(page.GroupName) ? a.GroupName == page.GroupName : true)
                .Where(a => !string.IsNullOrEmpty(page.RoleName) ? a.RoleName == page.RoleName : true)
                .Where(a => !string.IsNullOrEmpty(page.UserName) ? a.UserName.Contains(page.UserName) : true);

            var total = query.Count(); // 总记录数
            var list = query.Skip((page.Page - 1) * page.Limit).Take(page.Limit).ToList(); // 分页查询
            return new PageDto().SetList(list).SetTotal(total);
        }

        /// <summary>
        /// 添加新分组
        /// </summary>
        /// <param name="group">分组实体对象</param>
        /// <returns>始终返回 0</returns>
        public int AddGroups(Groups group)
        {
            group.Status = 1; // 设置分组状态为启用
            _dbContext.Groups.Add(group);
            _dbContext.SaveChanges();
            return 0;
        }

        /// <summary>
        /// 根据ID删除分组
        /// </summary>
        /// <param name="id">分组ID</param>
        /// <returns>始终返回 0，分组不存在也返回 0</returns>
        public int DeleteGroups(int id)
        {
            var group = _dbContext.Groups.Find(id);
            if (group == null) return 0;
            _dbContext.Remove(group);
            _dbContext.SaveChanges();
            return 0;
        }

        /// <summary>
        /// 更新分组信息
        /// </summary>
        /// <param name="group">分组实体对象</param>
        /// <returns>始终返回 0</returns>
        public int UpdateGroups(Groups group)
        {
            _dbContext.Update(group);
            _dbContext.SaveChanges();
            return 0;
        }

        /// <summary>
        /// 更新分组状态（启用/禁用）
        /// </summary>
        /// <param name="group">分组实体对象，包含要更新的ID和新状态</param>
        /// <returns>始终返回 0</returns>
        public int UpdateGroupsStatus(Groups group)
        {
            var updatedata = _dbContext.Groups.FirstOrDefault(g => g.Id == group.Id);
            if (updatedata != null)
            {
                updatedata.Status = group.Status; // 仅更新状态字段
                _dbContext.Update(updatedata);
                _dbContext.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// 获取所有分组列表
        /// </summary>
        /// <returns>分组列表</returns>
        public List<Groups> GetGroups()
        {
            return _dbContext.Groups.ToList();
        }

        /// <summary>
        /// 获取分组分页列表，支持按分组名称筛选
        /// </summary>
        /// <param name="page">分页参数，包含筛选条件和分页信息</param>
        /// <returns>包含分组列表和总数的分页结果 DTO</returns>
        public PageDto GetGroupsPage(PageParams page)
        {
            // 根据分组名称进行模糊筛选
            var query = _dbContext.Groups
                .Where(a => !string.IsNullOrEmpty(page.GroupName) ? a.GroupName.Contains(page.GroupName) : true);

            var total = query.Count(); // 总记录数
            var list = query.Skip((page.Page - 1) * page.Limit).Take(page.Limit).ToList(); // 分页查询
            return new PageDto().SetList(list).SetTotal(total);
        }

        /// <summary>
        /// 修改用户密码。需验证旧密码正确后才能设置新密码。
        /// </summary>
        /// <param name="changePassword">密码修改 DTO，包含用户名、旧密码和新密码</param>
        /// <returns>成功返回 0，旧密码验证失败返回 1</returns>
        public int ChangePassword(ChangePasswordDto changePassword)
        {
            // 查找用户并验证旧密码
            var user = _dbContext.Users.FirstOrDefault(a => a.UserName == changePassword.UserName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(changePassword.OldPassword, user.Password))
                return 1;

            // 使用 BCrypt 对新密码进行哈希加密并保存
            user.Password = BCrypt.Net.BCrypt.HashPassword(changePassword.NewPassword);
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
            return 0;
        }

        /// <summary>
        /// 初始化种子数据。当数据库中无用户数据时，自动创建默认角色、用户和分组。
        /// </summary>
        public void SeedData()
        {
            // 若已有用户数据则跳过初始化
            if (_dbContext.Users.Any()) return;

            // 初始化默认角色：管理员、教师、学生
            var roles = new List<Roles>
            {
                new() { RoleName = "管理员", Status = 1 },
                new() { RoleName = "教师", Status = 1 },
                new() { RoleName = "学生", Status = 1 }
            };
            _dbContext.Roles.AddRange(roles);
            _dbContext.SaveChanges();

            // 初始化默认用户，密码统一为 123456 的 BCrypt 哈希值
            var users = new List<Users>
            {
                new() { UserName = "admin", Password = BCrypt.Net.BCrypt.HashPassword("123456"), RoleName = "管理员", GroupName = "系统组", Status = 1 },
                new() { UserName = "teacher", Password = BCrypt.Net.BCrypt.HashPassword("123456"), RoleName = "教师", GroupName = "教研组", Status = 1 },
                new() { UserName = "student", Password = BCrypt.Net.BCrypt.HashPassword("123456"), RoleName = "学生", GroupName = "学习组", Status = 1 }
            };
            _dbContext.Users.AddRange(users);
            _dbContext.SaveChanges();

            // 初始化默认分组
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
