using Budget.Controllers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Budget.Models
{
    public class BudgetContext : IdentityDbContext<User>
    {
        public DbSet<OperationsWithMoney> OperationsWithMoney { get; set; }
        public DbSet<GetMoney> GetMoney { get; set; }
        public DbSet<SpendMoney> SpendMoney { get; set; }
        public DbSet<MoneyForEveryone> MoneyForEveryone { get; set; }

        public BudgetContext(DbContextOptions<BudgetContext> options) : base(options)
            => Database.EnsureCreated();
    }
}
