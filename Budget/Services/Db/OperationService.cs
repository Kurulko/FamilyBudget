using Budget.Models.Database;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading.Tasks;

namespace Budget.Services.Db;

public class OperationService : DbModelService<Operation>
{
    public OperationService(BudgetContext db) : base(db) { }

    protected override DbSet<Operation> models => db.Operations;
}
