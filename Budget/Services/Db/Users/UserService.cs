using Budget.Models.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Budget.Services.Db.Users;

public class UserService : AbsUserService
{
    readonly UserManager<User> userManager;
    public UserService(UserManager<User> userManager, BudgetContext db) : base(db)
        => this.userManager = userManager;

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
}
