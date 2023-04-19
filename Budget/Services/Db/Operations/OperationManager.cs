using Budget.Models.Database;
using Budget.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Budget.Models.Database.User;

namespace Budget.Services.Db.Operations;

public class OperationManager : OperationService
{
    public OperationManager(BudgetContext db) : base(db) { }

    protected override DbSet<Operation> models => db.Operations;

    IIncludableQueryable<Operation, dynamic> ModelsWithInclude()
        => models.Include(o => o.Money).ThenInclude(m => m!.Currency).Include(o => o.Category).ThenInclude(c => c!.ChildCategories).Include(c => c.SubCategory).Include(o => o.User)!;

    public override Task<Operation?> GetModelByIdAsync(long id)
        => Task.FromResult(ModelsWithInclude().ToList().FirstOrDefault(o => PredicateForModelById(id)(o)));

    public override Task<IEnumerable<Operation>> GetModelsAsync()
        => Task.FromResult(ModelsWithInclude().ToList().Where(o => PredicateForModels()(o)));

    public override Task<IEnumerable<Operation>> GetModelsByPredicateAsync(Func<Operation, bool> predicate)
        => Task.FromResult(ModelsWithInclude().ToList().Where(o => PredicateForModels()(o) && predicate(o)));

    record MoneyGroupModel(TypeOfMoney TypeOfMoney, char CurrencySymbol);
    record OperationGroupModel(decimal Money, TypeOfOperation TypeOfOperation);
    public override IEnumerable<GroupMoney> GetCurrentSumsOfMoney()
    {
        IEnumerable<Operation> operations = models.Include(u => u.Money).ThenInclude(m => m!.Currency).ToList().Where(u => PredicateForModels()(u));

        IEnumerable<GroupMoney> groupsMoney = operations.GroupBy(
            o => new MoneyGroupModel(o.Money!.TypeOfMoney, o.Money!.Currency!.Symbol),
            o => new OperationGroupModel(o.Money!.Price, o.TypeOfOperation),
            (mm, operations) =>
            {
                decimal sumOfMoney = operations.Select(o => o.Money * (o.TypeOfOperation == TypeOfOperation.Purchase ? -1 : 1)).Sum();
                return new GroupMoney(mm.TypeOfMoney, mm.CurrencySymbol, sumOfMoney);
            });

        return groupsMoney;
    }

    record MonthAndYear(int Month, int Year);
    IEnumerable<GroupOperation> GetGroupsOperation(IEnumerable<Operation> operations)
        => operations.GroupBy(o => new MonthAndYear(o.DateTime.Month, o.DateTime.Year),
            (MonthAndYear may, IEnumerable<Operation> ops) => new GroupOperation() { Month = may.Month, Year = may.Year, Operations = ops });

    public override async Task<IEnumerable<GroupOperation>> GetGroupsOperationAsync()
        => GetGroupsOperation(await GetModelsAsync());

    public override async Task<IEnumerable<GroupOperation>> GetGroupsOperationByPredicateAsync(Func<Operation, bool> predicate)
        => GetGroupsOperation(await GetModelsByPredicateAsync(predicate));
}
