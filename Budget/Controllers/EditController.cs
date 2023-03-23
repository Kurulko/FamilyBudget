using Budget.Models.Database;
using Budget.Services.Db;
using Budget.Services.Db.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Controllers;

[Authorize]
public abstract class EditController<TModel, TId> : BudgetController where TModel : class
{
    protected readonly Service<TModel, TId> dbService;
    public EditController(AbsUserService userService, Service<TModel, TId> dbService) : base(userService)
        => this.dbService = dbService;


    [HttpGet, ActionName("Add")]
    public abstract Task<IActionResult> AddAsync();
    [HttpPost, ActionName("Add")]
    public abstract Task<IActionResult> AddAsync(TModel model);

    [HttpGet, ActionName("Edit")]
    public abstract Task<IActionResult> EditAsync(TId modelId);
    [HttpPost, ActionName("Edit")]
    public abstract Task<IActionResult> EditAsync(TModel model);

    [HttpGet, ActionName("Delete")]
    public abstract Task<IActionResult> DeleteAsync(TId modelId);
}
