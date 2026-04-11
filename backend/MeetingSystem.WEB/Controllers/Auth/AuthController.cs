using MeetingSystem.Common.Utils;
using MeetingSystem.IService.Auth;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace MeetingSystem.WEB.Controllers.Auth;

/// <summary>
/// 认证与权限管理控制器，提供用户登录/登出、角色管理、用户管理、分组管理及密码修改等功能
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// 构造函数，通过依赖注入获取认证服务实例
    /// </summary>
    /// <param name="authService">认证服务接口</param>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// 用户登录接口，验证用户名和密码
    /// </summary>
    /// <param name="loginParams">登录参数，包含用户名和密码</param>
    /// <returns>登录结果，成功返回用户信息，失败返回错误提示</returns>
    [HttpPost]
    public R Login(LoginParams loginParams)
    {
        // 调用认证服务进行登录验证
        var result = _authService.Login(loginParams.UserName, loginParams.Pwd);
        // Code == 1 表示登录失败
        if (result.Code == 1)
            return new R().Error().SetMessage("用户名或密码错误");
        return new R().OK().SetData(result);
    }

    /// <summary>
    /// 用户登出接口
    /// </summary>
    /// <param name="userName">要登出的用户名</param>
    /// <returns>登出结果</returns>
    [HttpPost]
    public R Logout(string userName)
    {
        return new R().OK().SetData(_authService.Logout(userName));
    }

    /// <summary>
    /// 添加新角色
    /// </summary>
    /// <param name="role">角色信息</param>
    /// <returns>添加结果</returns>
    [HttpPost]
    public R AddRoles(Roles role) => new R().OK().SetData(_authService.AddRoles(role));

    /// <summary>
    /// 根据ID删除角色
    /// </summary>
    /// <param name="id">角色ID</param>
    /// <returns>删除结果</returns>
    [HttpPost]
    public R DeleteRoles(int id) => new R().OK().SetData(_authService.DeleteRoles(id));

    /// <summary>
    /// 更新角色信息
    /// </summary>
    /// <param name="role">角色信息</param>
    /// <returns>更新结果</returns>
    [HttpPost]
    public R UpdateRoles(Roles role) => new R().OK().SetData(_authService.UpdateRoles(role));

    /// <summary>
    /// 根据ID获取单个角色信息
    /// </summary>
    /// <param name="id">角色ID</param>
    /// <returns>角色信息</returns>
    [HttpGet]
    public R GetRoles(int id) => new R().OK().SetData(_authService.GetRoles(id));

    /// <summary>
    /// 获取所有角色列表
    /// </summary>
    /// <returns>角色列表</returns>
    [HttpPost]
    public R GetRoles() => new R().OK().SetData(_authService.GetRoles());

    /// <summary>
    /// 分页获取角色列表
    /// </summary>
    /// <param name="page">分页参数</param>
    /// <returns>分页角色数据</returns>
    [HttpPost]
    public R GetRolesPage(PageParams page) => new R().OK().SetData(_authService.GetRolesPage(page));

    /// <summary>
    /// 添加新用户
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns>添加结果</returns>
    [HttpPost]
    public R AddUsers(Users user) => new R().OK().SetData(_authService.AddUsers(user));

    /// <summary>
    /// 根据ID删除用户
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>删除结果</returns>
    [HttpGet]
    public R DeleteUsers(int id) => new R().OK().SetData(_authService.DeleteUsers(id));

    /// <summary>
    /// 更新用户信息
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns>更新结果</returns>
    [HttpPost]
    public R UpdateUsers(Users user) => new R().OK().SetData(_authService.UpdateUsers(user));

    /// <summary>
    /// 获取所有用户列表
    /// </summary>
    /// <returns>用户列表</returns>
    [HttpPost]
    public R GetUsers() => new R().OK().SetData(_authService.GetUsers());

    /// <summary>
    /// 根据ID获取单个用户信息
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>用户信息</returns>
    [HttpGet]
    public R GetUsers(int id) => new R().OK().SetData(_authService.GetUsers(id));

    /// <summary>
    /// 分页获取用户列表
    /// </summary>
    /// <param name="page">分页参数</param>
    /// <returns>分页用户数据</returns>
    [HttpPost]
    public R GetUsersPage(PageParams page) => new R().OK().SetData(_authService.GetUsersPage(page));

    /// <summary>
    /// 添加新分组
    /// </summary>
    /// <param name="group">分组信息</param>
    /// <returns>添加结果</returns>
    [HttpPost]
    public R AddGroups(Groups group) => new R().OK().SetData(_authService.AddGroups(group));

    /// <summary>
    /// 根据ID删除分组
    /// </summary>
    /// <param name="id">分组ID</param>
    /// <returns>删除结果</returns>
    [HttpGet]
    public R DeleteGroups(int id) => new R().OK().SetData(_authService.DeleteGroups(id));

    /// <summary>
    /// 更新分组信息
    /// </summary>
    /// <param name="group">分组信息</param>
    /// <returns>更新结果</returns>
    [HttpPost]
    public R UpdateGroups(Groups group) => new R().OK().SetData(_authService.UpdateGroups(group));

    /// <summary>
    /// 更新分组状态（启用/禁用）
    /// </summary>
    /// <param name="group">分组信息（包含状态字段）</param>
    /// <returns>更新结果</returns>
    [HttpPost]
    public R UpdateGroupsStatus(Groups group) => new R().OK().SetData(_authService.UpdateGroupsStatus(group));

    /// <summary>
    /// 分页获取分组列表
    /// </summary>
    /// <param name="page">分页参数</param>
    /// <returns>分页分组数据</returns>
    [HttpPost]
    public R GetGroupsPage(PageParams page) => new R().OK().SetData(_authService.GetGroupsPage(page));

    /// <summary>
    /// 修改用户密码
    /// </summary>
    /// <param name="dto">密码修改参数，包含旧密码和新密码</param>
    /// <returns>修改结果，返回0表示成功</returns>
    [HttpPost]
    public R ChangePassword(ChangePasswordDto dto)
    {
        // 调用服务层执行密码修改
        var res = _authService.ChangePassword(dto);
        // res == 0 表示修改成功
        return res == 0
            ? new R().OK().SetData(res).SetMessage("密码修改成功")
            : new R().Error().SetMessage("密码修改失败");
    }

    /// <summary>
    /// 初始化种子数据，用于系统首次部署时填充默认数据
    /// </summary>
    /// <returns>初始化结果</returns>
    [HttpPost]
    public R SeedData()
    {
        try
        {
            _authService.SeedData();
            return new R().OK().SetMessage("种子数据初始化成功");
        }
        catch (Exception ex)
        {
            // 捕获异常并返回错误信息
            return new R().Error().SetMessage($"初始化失败: {ex.Message}");
        }
    }
}
