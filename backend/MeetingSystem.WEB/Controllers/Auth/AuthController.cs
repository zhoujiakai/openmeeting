using MeetingSystem.Common.Utils;
using MeetingSystem.IService.Auth;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace MeetingSystem.WEB.Controllers.Auth;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public R Login(LoginParams loginParams)
    {
        var result = _authService.Login(loginParams.UserName, loginParams.Pwd);
        if (result.Code == 1)
            return new R().Error().SetMessage("用户名或密码错误");
        return new R().OK().SetData(result);
    }

    [HttpPost]
    public R Logout(string userName)
    {
        return new R().OK().SetData(_authService.Logout(userName));
    }

    [HttpPost]
    public R AddRoles(Roles role) => new R().OK().SetData(_authService.AddRoles(role));

    [HttpPost]
    public R DeleteRoles(int id) => new R().OK().SetData(_authService.DeleteRoles(id));

    [HttpPost]
    public R UpdateRoles(Roles role) => new R().OK().SetData(_authService.UpdateRoles(role));

    [HttpGet]
    public R GetRoles(int id) => new R().OK().SetData(_authService.GetRoles(id));

    [HttpPost]
    public R GetRoles() => new R().OK().SetData(_authService.GetRoles());

    [HttpPost]
    public R GetRolesPage(PageParams page) => new R().OK().SetData(_authService.GetRolesPage(page));

    [HttpPost]
    public R AddUsers(Users user) => new R().OK().SetData(_authService.AddUsers(user));

    [HttpGet]
    public R DeleteUsers(int id) => new R().OK().SetData(_authService.DeleteUsers(id));

    [HttpPost]
    public R UpdateUsers(Users user) => new R().OK().SetData(_authService.UpdateUsers(user));

    [HttpPost]
    public R GetUsers() => new R().OK().SetData(_authService.GetUsers());

    [HttpGet]
    public R GetUsers(int id) => new R().OK().SetData(_authService.GetUsers(id));

    [HttpPost]
    public R GetUsersPage(PageParams page) => new R().OK().SetData(_authService.GetUsersPage(page));

    [HttpPost]
    public R AddGroups(Groups group) => new R().OK().SetData(_authService.AddGroups(group));

    [HttpGet]
    public R DeleteGroups(int id) => new R().OK().SetData(_authService.DeleteGroups(id));

    [HttpPost]
    public R UpdateGroups(Groups group) => new R().OK().SetData(_authService.UpdateGroups(group));

    [HttpPost]
    public R UpdateGroupsStatus(Groups group) => new R().OK().SetData(_authService.UpdateGroupsStatus(group));

    [HttpPost]
    public R GetGroupsPage(PageParams page) => new R().OK().SetData(_authService.GetGroupsPage(page));

    [HttpPost]
    public R ChangePassword(ChangePasswordDto dto)
    {
        var res = _authService.ChangePassword(dto);
        return res == 0
            ? new R().OK().SetData(res).SetMessage("密码修改成功")
            : new R().Error().SetMessage("密码修改失败");
    }

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
            return new R().Error().SetMessage($"初始化失败: {ex.Message}");
        }
    }
}
