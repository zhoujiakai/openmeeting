using MeetingSystem.IService.Base;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.Auth;

namespace MeetingSystem.IService.Auth
{
    /// <summary>
    /// 认证与授权服务接口，提供登录、注销、用户管理、角色管理、分组管理等功能
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录结果信息</returns>
        public LoginDto Login(string username, string password);

        /// <summary>
        /// 用户注销登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>操作结果，成功返回0</returns>
        public int Logout(string username);

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns>受影响的行数</returns>
        public int AddRoles(Roles role);

        /// <summary>
        /// 根据ID删除角色
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>受影响的行数</returns>
        public int DeleteRoles(int id);

        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns>受影响的行数</returns>
        public int UpdateRoles(Roles role);

        /// <summary>
        /// 根据ID获取角色信息
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>角色信息</returns>
        public Roles GetRoles(int id);

        /// <summary>
        /// 获取所有角色列表
        /// </summary>
        /// <returns>角色列表</returns>
        public List<Roles> GetRoles();

        /// <summary>
        /// 分页查询角色列表
        /// </summary>
        /// <param name="page">分页参数</param>
        /// <returns>分页结果</returns>
        public PageDto GetRolesPage(PageParams page);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>受影响的行数</returns>
        public int AddUsers(Users user);

        /// <summary>
        /// 根据ID删除用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>受影响的行数</returns>
        public int DeleteUsers(int id);

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>受影响的行数</returns>
        public int UpdateUsers(Users user);

        /// <summary>
        /// 根据ID获取用户信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>用户信息</returns>
        public Users GetUsers(int id);

        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        /// <returns>用户列表</returns>
        public List<Users> GetUsers();

        /// <summary>
        /// 分页查询用户列表
        /// </summary>
        /// <param name="page">分页参数</param>
        /// <returns>分页结果</returns>
        public PageDto GetUsersPage(PageParams page);

        /// <summary>
        /// 添加分组
        /// </summary>
        /// <param name="group">分组信息</param>
        /// <returns>受影响的行数</returns>
        public int AddGroups(Groups group);

        /// <summary>
        /// 根据ID删除分组
        /// </summary>
        /// <param name="id">分组ID</param>
        /// <returns>受影响的行数</returns>
        public int DeleteGroups(int id);

        /// <summary>
        /// 更新分组信息
        /// </summary>
        /// <param name="group">分组信息</param>
        /// <returns>受影响的行数</returns>
        public int UpdateGroups(Groups group);

        /// <summary>
        /// 更新分组状态
        /// </summary>
        /// <param name="group">分组信息（包含状态）</param>
        /// <returns>受影响的行数</returns>
        public int UpdateGroupsStatus(Groups group);

        /// <summary>
        /// 获取所有分组列表
        /// </summary>
        /// <returns>分组列表</returns>
        public List<Groups> GetGroups();

        /// <summary>
        /// 分页查询分组列表
        /// </summary>
        /// <param name="page">分页参数</param>
        /// <returns>分页结果</returns>
        public PageDto GetGroupsPage(PageParams page);

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="changePassword">修改密码参数</param>
        /// <returns>受影响的行数</returns>
        public int ChangePassword(ChangePasswordDto changePassword);

        /// <summary>
        /// 初始化种子数据
        /// </summary>
        public void SeedData();
    }
}
