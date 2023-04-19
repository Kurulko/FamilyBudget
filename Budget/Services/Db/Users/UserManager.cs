using Budget.Models.Database;
using Budget.Models.ViewModel;
using Budget.Models.ViewModel.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Threading.Tasks;
using static Budget.Models.Database.User;

namespace Budget.Services.Db.Users;

public class UserManager : UserService
{
    public UserManager(UserManager<User> userManager, BudgetContext db) : base(userManager, db) { }

    protected override DbSet<User> models => db.Users;

    public override async Task AddRoleToUserAsync(string roleName, string userId)
    {
        User user = (await GetModelByIdAsync(userId))!;
        await userManager.AddToRoleAsync(user, roleName);
    }

    public override async Task AddUserPasswordAsync(ChangePassword changePassword, string userId)
    {
        User user = (await GetModelByIdAsync(userId))!;
        await userManager.AddPasswordAsync(user, changePassword.NewPassword);
    }

    public override async Task ChangeUserPasswordAsync(ChangePassword changePassword, string userId)
    {
        User user = (await GetModelByIdAsync(userId))!;
        await userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
    }

    public override async Task DeleteRoleFromUserAsync(string userId, string roleName)
    {
        User user = (await GetModelByIdAsync(userId))!;
        await userManager.RemoveFromRoleAsync(user, roleName);
    }

    public override async Task<IEnumerable<string>> GetRolesAsync(string userId)
    {
        User user = (await GetModelByIdAsync(userId))!;
        return await userManager.GetRolesAsync(user);
    }

    public override string? GetUserIdByClaims(ClaimsPrincipal userClaims)
    {
        try
        {
            return userManager.GetUserId(userClaims);
        }
        catch
        {
            return default;
        }
    }

    public override async Task<bool> HasUserPasswordAsync(string userId)
    {
        User user = (await GetModelByIdAsync(userId))!;
        return await userManager.HasPasswordAsync(user);
    }

    protected override User CreateModelById(string modelId)
        => new() { Id = modelId };

    protected override Func<User, bool> PredicateForModelById(string modelId)
        => (User u) => u.Id == modelId;

    protected override Func<User, bool> PredicateForModels()
        => (User u) => true;
}
