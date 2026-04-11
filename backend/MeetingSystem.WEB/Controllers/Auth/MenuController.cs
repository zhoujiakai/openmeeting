using MeetingSystem.Common.Utils;
using MeetingSystem.IService.Auth;
using MeetingSystem.Model.Dto;
using MeetingSystem.Model.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace MeetingSystem.WEB.Controllers.Auth;

[ApiController]
[Route("api/[controller]/[action]")]
public class MenuController : ControllerBase
{
    private readonly IMenuItemService _menuItemService;

    public MenuController(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    [HttpPost]
    public R Add(MenuItems menuItem) => new R().OK().SetData(_menuItemService.Add(menuItem));

    [HttpGet]
    public R Del(int id) => new R().OK().SetData(_menuItemService.Remove(id));

    [HttpPost]
    public R Update(MenuItems menuItem) => new R().OK().SetData(_menuItemService.Update(menuItem));

    [HttpPost]
    public R GetUserMenuList(MenuParams menuParams)
        => new R().OK().SetData(_menuItemService.GetUserMenuList(menuParams.RoleName));

    [HttpPost]
    public R List() => new R().OK().SetData(_menuItemService.List());
}
