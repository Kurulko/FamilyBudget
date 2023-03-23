using Budget.Models.Database;
using Budget.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Controllers;

[Authorize]
public abstract class EditController<TModel, TId> : BudgetController
{
    protected readonly BudgetContext db;
    public EditController(IUserService userService, BudgetContext db) : base(userService)
        => this.db = db;


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
