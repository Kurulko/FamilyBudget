using Budget.Models.Database;
using Budget.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Budget.Models.Database.User;

namespace Budget.Services.Db;

public class OperationService : DbModelService<Operation>
{
    public OperationService(BudgetContext db) : base(db) { }

    protected override DbSet<Operation> models => db.Operations;

    IIncludableQueryable<Operation, dynamic> ModelsWithInclude()
        => models.Include(o => o.Money).ThenInclude(m => m!.Currency).Include(o => o.Category).ThenInclude(c => c!.ChildCategories).Include(o => o.User)!;

    public override Task<Operation?> GetModelByIdAsync(long id)
        => Task.FromResult(ModelsWithInclude().ToList().FirstOrDefault(o => PredicateForModelById(id)(o)));

    public override Task<IEnumerable<Operation>> GetModelsAsync()
        => Task.FromResult(ModelsWithInclude().ToList().Where(o => PredicateForModels()(o)));

    public override Task<IEnumerable<Operation>> GetModelsByPredicateAsync(Func<Operation, bool> predicate)
        => Task.FromResult(ModelsWithInclude().ToList().Where(o => PredicateForModels()(o) && predicate(o)));

    record MoneyGroupModel(TypeOfMoney TypeOfMoney, char CurrencySymbol);
    record OperationGroupModel(decimal Money, TypeOfOperation TypeOfOperation);
    public IEnumerable<GroupMoney> GetCurrentSumsOfMoney()
    {
        IEnumerable<Operation> operations = models.Include(u => u.Money).ThenInclude(m => m!.Currency).ToList().Where(u => PredicateForModels()(u));

        IEnumerable<GroupMoney> groupsMoney = operations.GroupBy(
            o => new MoneyGroupModel(o.Money!.TypeOfMoney, o.Money!.Currency!.Symbol),
            o => new OperationGroupModel(o.Money!.Count, o.TypeOfOperation),
            (MoneyGroupModel mm, IEnumerable<OperationGroupModel> operations) =>
            {
                decimal sumOfMoney = operations.Select(o => o.Money * (o.TypeOfOperation == TypeOfOperation.Purchase ? -1 : 1)).Sum();
                return new GroupMoney(mm.TypeOfMoney, mm.CurrencySymbol, sumOfMoney);
            });

        return groupsMoney;
    }
}
