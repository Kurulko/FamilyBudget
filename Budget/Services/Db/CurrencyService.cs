using Budget.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace Budget.Services.Db;

public class CurrencyService : DbModelService<Currency>
{
    public CurrencyService(BudgetContext db) : base(db) { }

    protected override DbSet<Currency> models => db.Currencies;
}
