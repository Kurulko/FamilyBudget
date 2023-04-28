using Budget.Services.Db.Users;
using Budget.Services.Db;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Budget.Models.Database;
using Budget.Models.ViewModel.Account;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Budget.Models.ViewModel.Helpers;
using Budget.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Budget.Services.Db.Categories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Budget.Controllers.DB;

[Route(name)]
[Authorize(Roles = Roles.Admin)]
public sealed class UserController : ActiveEditController<User, string>
{
    public UserController(UserService userService) : base(userService, userService) { }

    internal const string name = "users";

    protected override IActionResult RedirectToBack()
        => Redirect($"/{name}/{pathToModels}");

    string userId => userService.GetUserIdByClaims(User)!;

    string GetUserId(string? userId)
        => userId is null || !User.IsInRole(Roles.Admin) ? this.userId : userId;

    internal const string viewPasswordName = "Password";
    internal const string partPathToPassword = "password";

    internal const string pathToChangePassword = $"{partPathToPassword}/{partPathEdit}";
    internal const string pathToChangePasswordWithUserId = $"{partPathToUserId}/{pathToChangePassword}";

    [HttpGet(pathToChangePassword)]
    [HttpGet(pathToChangePasswordWithUserId)]
    [Authorize]
    public Task<IActionResult> ChangePasswordAsync(string? userId)
        => GetActionResultAsync(View(viewPasswordName, new ModelWithMode<ChangePasswordWithUserId>(new ChangePasswordWithUserId() { UserId = GetUserId(userId) }, Mode.Edit)));

    [HttpPost(pathToChangePassword)]
    [HttpPost(pathToChangePasswordWithUserId)]
    [Authorize]
    public Task<IActionResult> ChangePasswordAsync(ChangePassword changePassword, string? userId)
    {
        var modelWithMode = new ModelWithMode<ChangePasswordWithUserId>(new ChangePasswordWithUserId() { ChangePassword = changePassword, UserId = userId }, Mode.Edit);
        
        return DoActionIfValid(async cpWithUserId => {
            var result = await userService.ChangeUserPasswordAsync(cpWithUserId.ChangePassword, GetUserId(userId));
            
            if(!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(viewPasswordName, modelWithMode);
            }

            return RedirectToBack();

        }, modelWithMode, viewPasswordName);
    }


    [HttpPost($"{partPathToPassword}/{partPathAdd}")]
    [HttpPost($"{partPathToUserId}/{partPathToPassword}/{partPathAdd}")]
    public async Task<IActionResult> AddPasswordAsync(ChangePassword changePassword, string? userId)
    {
        await userService.AddUserPasswordAsync(changePassword, GetUserId(userId));
        return RedirectToBack();
    }


    internal const string viewRolesName = "Roles";
    internal const string partPathToRoles = "roles";

    internal const string pathToChangeRolesWithUserId = $"{partPathToUserId}/{partPathToRoles}/{partPathEdit}";

    [HttpGet(pathToChangeRolesWithUserId)]
    public async Task<IActionResult> EditRolesAsync(string? userId, [FromServices]Service<IdentityRole, string> roleService)
    {
        var allroles = await roleService.GetModelsAsync();
        ViewBag.Roles = new SelectList(allroles, nameof(IdentityRole.Name), nameof(IdentityRole.Name));
        IEnumerable<string> userRoles = await userService.GetRolesAsync(GetUserId(userId));
        return View(viewRolesName, new RolesAndUserId() { Roles = userRoles, UserId = userId});
    }

    [HttpPost(pathToChangeRolesWithUserId)]
    public async Task<IActionResult> EditRolesAsync(string? userId, IEnumerable<string> roles, [FromServices] Service<IdentityRole, string> roleService)
    {
        if (ModelState.IsValid)
        {
            await userService.AddNewAndDeleteOldRolesFromUserAsync(GetUserId(userId), roles);
            return await GetActionResultAsync(RedirectToBack());
        }

        ViewBag.Roles = new SelectList(await roleService.GetModelsAsync(), nameof(IdentityRole.Name), nameof(IdentityRole.Name));
        return View(viewRolesName, new RolesAndUserId() { Roles = roles, UserId = userId });
    }


    [HttpPost($"{partPathToRoles}/{partPathAdd}")]
    public async Task<IActionResult> AddRoleAsync(string? userId, string roleName)
    {
        await userService.AddRoleToUserAsync(roleName, GetUserId(userId));
        return RedirectToBack();
    }

    const string partPathToUserId = "{userId}";
    const string partPathToRoleName = "{roleName}";

    [HttpGet($"{partPathToUserId}/{partPathToRoles}/{partPathDelete}/{partPathToRoleName}")]
    public async Task<IActionResult> DeleteRoleAsync(string? userId, string roleName)
    {
        await userService.DeleteRoleFromUserAsync(roleName, GetUserId(userId));
        return RedirectToBack();
    }


    [HttpGet("/me")]
    [HttpGet(pathToModelById)]
    public override async Task<IActionResult> GetModelByIdAsync(string? id)
    {
        if(User.IsInRole(Roles.Admin) || id is null || this.userId == id)
            return await base.GetModelByIdAsync(GetUserId(id));

        return RedirectToBack();
    }
}

