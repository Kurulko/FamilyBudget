using Budget.Controllers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Budget.Models.Database;

public class BudgetContext : IdentityDbContext<User>
{
    public DbSet<Money> Money { get; set; } = null!;
    public DbSet<Currency> Currencies { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Operation> Operations { get; set; } = null!;

    public BudgetContext(DbContextOptions<BudgetContext> options) : base(options)
        => Database.EnsureCreated();
}
