using Budget.Models.Database;
using Budget.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Budget.Services.Db.Operations.OperationManager;

namespace Budget.Services.Db.Operations;

public abstract class OperationService : DbModelService<Operation>
{
    public OperationService(BudgetContext db) : base(db) { }

    public abstract IEnumerable<GroupMoney> GetCurrentSumsOfMoney();
    public abstract IEnumerable<GroupMoneyForMonth> GetCurrentSumsOfMoneyForMonth();
    public abstract Task<IEnumerable<GroupOperation>> GetGroupsOperationAsync();
    public abstract Task<IEnumerable<GroupOperation>> GetGroupsOperationByPredicateAsync(Func<Operation, bool> predicate);
}
