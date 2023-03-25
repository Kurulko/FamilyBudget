using Budget.Models.Database;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Budget.Services.Db.Users;

public abstract class AbsUserService : Service<User, string>
{
    public AbsUserService(BudgetContext db) : base(db) { }

    protected override DbSet<User> models => db.Users;
    public override void DeleteModelById(string id)
    {
        models.Remove(new() { Id = id });
        SaveChanges();
    }

    public override async Task<User?> GetModelByIdAsync(string id)
        => await models.FirstOrDefaultAsync(o => o.Id == id);

    public abstract string? GetUserIdByClaims(ClaimsPrincipal userClaims);
}
