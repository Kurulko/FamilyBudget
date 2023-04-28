using Budget.Models.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Budget.Models.ViewModel.Account;
using static Budget.Models.Database.User;
using Budget.Models.ViewModel;

namespace Budget.Services.Db.Users;

public abstract class UserService : Service<User, string>
{
    protected readonly UserManager<User> userManager;
    public UserService(UserManager<User> userManager, BudgetContext db) : base(db)
        => this.userManager = userManager;

    public abstract string? GetUserIdByClaims(ClaimsPrincipal userClaims);
    public abstract Task<IdentityResult> ChangeUserPasswordAsync(ChangePassword changePassword, string userId);
    public abstract Task<IdentityResult> AddUserPasswordAsync(ChangePassword changePassword, string userId);
    public abstract Task<bool> HasUserPasswordAsync(string userId);
    public abstract Task<bool> IsInRoleAsync(string userId, string roleName);
    public abstract Task<IEnumerable<string>> GetRolesAsync(string userId);
    public abstract Task<IdentityResult> AddRoleToUserAsync(string role, string userId);
    public abstract Task<IdentityResult> DeleteRoleFromUserAsync(string userId, string roleName);
    public abstract Task AddNewAndDeleteOldRolesFromUserAsync(string userId, IEnumerable<string> roles);
}
