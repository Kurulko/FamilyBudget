using Budget.Models.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Budget.Services.Db;

public class MoneyManager : DbModelService<Money>
{
    public MoneyManager(BudgetContext db) : base(db) { }

    protected override DbSet<Money> models => db.Money;
}
