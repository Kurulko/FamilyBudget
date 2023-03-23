﻿using Budget.Models.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Budget.Services.Db;

public abstract class Service<TModel, TId> where TModel : class
{
    protected readonly BudgetContext db;
    public Service(BudgetContext db)
        => this.db = db;
    protected abstract DbSet<TModel> models { get; }

    public virtual async Task AddModelAsync(TModel model)
    {
        await models.AddAsync(model);
        SaveChanges();
    }
    public virtual void EditModel(TModel model)
    {
        models.Update(model);
        SaveChanges();
    }

    public abstract void DeleteModel(TId id);
    public abstract Task<TModel?> GetModelByIdAsync(TId id);

    protected virtual void SaveChanges()
        => db.SaveChanges();
}