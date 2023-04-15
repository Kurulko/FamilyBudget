using Budget.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Budget.Services.Db;

public abstract class DbModelService<TModel> : Service<TModel, long> where TModel : DbModel, new()
{
    public DbModelService(BudgetContext db) : base(db) { }

    public string UserId { get; set; } = null!;


    protected override Func<TModel, bool> PredicateForModels()
        => (TModel model) => model.UserId == UserId;
    protected override Func<TModel, bool> PredicateForModelById(long modelId)
        => (TModel model) => PredicateForModels()(model) && model.Id == modelId;
    protected override TModel CreateModelById(long modelId)
        => new TModel() { Id = modelId, UserId = UserId };

    public virtual async Task<IEnumerable<TModel>> GetAllModelsAsync()
        => await models.ToListAsync();
}
