using Budget.Models.Database;
using Budget.Services.Db.Users;
using Budget.Services.Db;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Controllers.DB;

public abstract class DbModelEditController<TModel> : EditController<TModel, long> where TModel : DbModel, new()
{
    public DbModelEditController(AbsUserService userService, DbModelService<TModel> dbService) : base(userService, dbService) { }
    protected override IActionResult RedirectToBack()
        => RedirectToAction(nameof(GetModelsAsync));

    [HttpGet("model/{modelId:long}")]
    public override Task<IActionResult> GetModelByIdAsync(long modelId)
        => base.GetModelByIdAsync(modelId);

    [HttpGet("edit/{modelId:long}")]
    public override Task<IActionResult> EditAsync(long modelId)
        => base.EditAsync(modelId);

    [HttpDelete("delete/{modelId:long}")]
    public override Task<IActionResult> DeleteAsync(long modelId)
        => base.DeleteAsync(modelId);
}
