using MeetingSystem.Common.Utils;
using MeetingSystem.IService.Auth;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace MeetingSystem.WEB.Controllers.Auth;

/// <summary>
/// 菜单管理控制器，提供菜单项的增删改查及根据角色获取菜单列表等功能
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class MenuController : ControllerBase
{
    private readonly IMenuItemService _menuItemService;

    /// <summary>
    /// 构造函数，通过依赖注入获取菜单项服务实例
    /// </summary>
    /// <param name="menuItemService">菜单项服务接口</param>
    public MenuController(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    /// <summary>
    /// 添加菜单项
    /// </summary>
    /// <param name="menuItem">菜单项信息</param>
    /// <returns>添加结果</returns>
    [HttpPost]
    public R Add(MenuItems menuItem) => new R().OK().SetData(_menuItemService.Add(menuItem));

    /// <summary>
    /// 根据ID删除菜单项
    /// </summary>
    /// <param name="id">菜单项ID</param>
    /// <returns>删除结果</returns>
    [HttpGet]
    public R Del(int id) => new R().OK().SetData(_menuItemService.Remove(id));

    /// <summary>
    /// 更新菜单项信息
    /// </summary>
    /// <param name="menuItem">菜单项信息</param>
    /// <returns>更新结果</returns>
    [HttpPost]
    public R Update(MenuItems menuItem) => new R().OK().SetData(_menuItemService.Update(menuItem));

    /// <summary>
    /// 根据角色名称获取该角色对应的菜单列表
    /// </summary>
    /// <param name="menuParams">菜单查询参数，包含角色名称</param>
    /// <returns>该角色可见的菜单列表</returns>
    [HttpPost]
    public R GetUserMenuList(MenuParams menuParams)
        => new R().OK().SetData(_menuItemService.GetUserMenuList(menuParams.RoleName));

    /// <summary>
    /// 获取所有菜单项列表
    /// </summary>
    /// <returns>全部菜单项列表</returns>
    [HttpPost]
    public R List() => new R().OK().SetData(_menuItemService.List());
}
