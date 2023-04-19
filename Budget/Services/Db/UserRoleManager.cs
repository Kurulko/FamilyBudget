using Budget.Models.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Budget.Services.Db;

public class UserRoleManager : Service<IdentityRole, string>
{
    public UserRoleManager(BudgetContext db) : base(db) { }

    protected override DbSet<IdentityRole> models => db.Roles;

    protected override IdentityRole CreateModelById(string modelId)
        => new() { Id = modelId };

    protected override Func<IdentityRole, bool> PredicateForModelById(string modelId)
        => (r) => r.Id == modelId;

    protected override Func<IdentityRole, bool> PredicateForModels()
        => (r) => true;
}
