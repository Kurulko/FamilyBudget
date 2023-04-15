using Budget.Models.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Budget.Services.Db.Users;

public class RoleService : Service<IdentityRole, string>
{
    public RoleService(BudgetContext db) : base(db) { }

    protected override DbSet<IdentityRole> models => db.Roles;

    protected override IdentityRole CreateModelById(string modelId)
        => new() { Id = modelId };

    protected override Func<IdentityRole, bool> PredicateForModelById(string modelId)
        => (IdentityRole r) => r.Id == modelId;

    protected override Func<IdentityRole, bool> PredicateForModels()
        => (IdentityRole r) => true;
}
