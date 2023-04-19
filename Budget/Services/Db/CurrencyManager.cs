using Budget.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;

namespace Budget.Services.Db;

public class CurrencyManager : Service<Currency, long>
{
    public CurrencyManager(BudgetContext db) : base(db) { }

    protected override DbSet<Currency> models => db.Currencies;

    protected override Currency CreateModelById(long modelId)
        => new() { Id = modelId };

    protected override Func<Currency, bool> PredicateForModelById(long modelId)
        => (Currency c) => c.Id == modelId;

    protected override Func<Currency, bool> PredicateForModels()
        => (Currency c) => true;
}
