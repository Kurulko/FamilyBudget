using Budget.Models.Database;
using Budget.Services.Db.Users;
using Budget.Services.Db;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Controllers.DB;

public abstract class DbModelEditController<TModel> : EditController<TModel, long> where TModel : DbModel, new()
{
    public DbModelEditController(AbsUserService userService, DbModelService<TModel> dbService) : base(userService, dbService) { }

    protected string userId => userService.GetUserIdByClaims(User)!;
    protected virtual void SetUserId()
        => (service as DbModelService<TModel>)!.UserId = userId;

    protected override TModel CreateAddModel()
        => new() { UserId = userId };


    public override Task<IActionResult> GetModelByIdAsync(long id)
    {
        SetUserId();
        return base.GetModelByIdAsync(id);
    }

    public override Task<IActionResult> GetModelsAsync()
    {
        SetUserId();
        return base.GetModelsAsync();
    }

    public override Task<IActionResult> DeleteAsync(long id)
    {
        SetUserId();
        return base.DeleteAsync(id);
    }

    public override Task<IActionResult> EditAsync(long id)
    {
        SetUserId();
        return base.EditAsync(id);
    }
}
