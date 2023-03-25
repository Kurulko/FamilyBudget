using Budget.Models.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Budget.Services.Db;

public abstract class DbModelService<TModel> : Service<TModel, long> where TModel : DbModel, new()
{
    public DbModelService(BudgetContext db) : base(db) { }

    public override void DeleteModelById(long id)
    {
        models.Remove(new() { Id = id });
        SaveChanges();
    }

    public override async Task<TModel?> GetModelByIdAsync(long id)
        => await models.FirstOrDefaultAsync(o => o.Id == id);
}
