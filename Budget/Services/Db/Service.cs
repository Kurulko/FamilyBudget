using Budget.Models.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch.Operations;
using static Budget.Models.Database.User;

namespace Budget.Services.Db;

public abstract class Service<TModel, TId> where TModel : class
{
    protected readonly BudgetContext db;
    public Service(BudgetContext db)
        => this.db = db;

    protected abstract DbSet<TModel> models { get; }

    protected abstract Func<TModel, bool> PredicateForModels();
    protected abstract Func<TModel, bool> PredicateForModelById(TId modelId);
    protected abstract TModel CreateModelById(TId modelId);


    public virtual async Task<TModel?> GetModelByIdAsync(TId modelId)
        => await models.FirstOrDefaultAsync(o => PredicateForModelById(modelId)(o));

    public virtual async Task<TModel?> GetModelDetailsByIdAsync(TId modelId)
        => await GetModelByIdAsync(modelId);

    public virtual async Task<IEnumerable<TModel>> GetModelsAsync()
        => (await models.ToListAsync()).Where(m => PredicateForModels()(m));
    public virtual async Task<IEnumerable<TModel>> GetModelsByPredicateAsync(Func<TModel, bool> predicate)
        => (await models.ToListAsync()).Where(m => PredicateForModels()(m) && predicate(m));


    public virtual async Task AddModelAsync(TModel model)
    {
        await models.AddAsync(model);
        SaveChanges();
    }
    public virtual async Task AddRangeModelsAsync(IEnumerable<TModel> models)
    {
        foreach (TModel model in models)
            await AddModelAsync(model);
    }

    public virtual void EditModel(TModel model)
    {
        models.Update(model);
        SaveChanges();
    }

    public virtual void DeleteModelById(TId modelId)
    {
        models.Remove(CreateModelById(modelId));
        SaveChanges();
    }

    protected virtual void SaveChanges()
        => db.SaveChanges();
}
