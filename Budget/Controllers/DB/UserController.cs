using Budget.Services.Db.Users;
using Budget.Services.Db;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Budget.Models.Database;
using Budget.Models.ViewModel.Account;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Budget.Controllers.DB;

[Route(name)]
public class UserController : ActiveEditController<User, string>
{
    public UserController(UserService userService) : base(userService, userService) { }

    const string name = "users";

    protected override IActionResult RedirectToBack()
        => Redirect($"/{name}/{pathToModels}");

    string userId => userService.GetUserIdByClaims(User)!;

    string GetUserId(string? userId)
        => userId is null ? this.userId : userId;


    const string partPathToPassword = "password";

    [HttpPost($"{partPathToPassword}/{partPathEdit}")]
    public async Task<IActionResult> ChangePasswordAsync(ChangePassword changePassword, string? userId)
    {
        await userService.ChangeUserPasswordAsync(changePassword, GetUserId(userId));
        return RedirectToBack();
    }

    [HttpPost($"{partPathToPassword}/{partPathAdd}")]
    public async Task<IActionResult> AddPasswordAsync(ChangePassword changePassword, string? userId)
    {
        await userService.AddUserPasswordAsync(changePassword, GetUserId(userId));
        return RedirectToBack();
    }

    const string partPathToRoles = "roles";

    [Authorize(Roles = Roles.Admin)]
    [HttpPost($"{partPathToRoles}/{partPathAdd}")]
    public async Task<IActionResult> AddRole(string? userId, string roleName)
    {
        await userService.AddRoleToUserAsync(roleName, GetUserId(userId));
        return RedirectToBack();
    }

    const string partPathToUserId = "{userId}";
    const string partPathToRoleName = "{roleName}";

    [Authorize(Roles = Roles.Admin)]
    [HttpGet($"{partPathToUserId}/{partPathToRoles}/{partPathDelete}/{partPathToRoleName}")]
    public async Task<IActionResult> DeleteRole(string? userId, string roleName)
    {
        await userService.DeleteRoleFromUserAsync(roleName, GetUserId(userId));
        return RedirectToBack();
    }

    [HttpGet("/me")]
    public override Task<IActionResult> GetModelByIdAsync(string id)
        =>  base.GetModelByIdAsync(GetUserId(id));
}

