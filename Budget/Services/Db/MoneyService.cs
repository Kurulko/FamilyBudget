using Budget.Models.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Budget.Services.Db;

public class MoneyService : DbModelService<Money>
{
    public MoneyService(BudgetContext db) : base(db) { }

    protected override DbSet<Money> models => db.Money;
}
