using Budget.Services.Db.Users;
using Budget.Services.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Budget.Controllers.DB;

[Authorize]
public abstract class EditController<TModel, TId> : BudgetController where TModel : class
{
    protected readonly Service<TModel, TId> dbService;
    public EditController(AbsUserService userService, Service<TModel, TId> dbService) : base(userService)
        => this.dbService = dbService;

    [HttpGet("model")]
    public virtual async Task<IActionResult> GetModelByIdAsync(TId modelId)
    {
        TModel? model = await dbService.GetModelByIdAsync(modelId);

        if (model is null)
            return RedirectToBack();

        return View("Models", model);
    }

    [HttpGet("models")]
    public virtual async Task<IActionResult> GetModelsAsync()
            => View("Models", await dbService.GetModelsAsync());


    [HttpGet("add")]
    public virtual Task<IActionResult> AddAsync()
        => GetActionResultAsync(View());

    [HttpPost("add")]
    public Task<IActionResult> AddAsync(TModel model)
        => DoActionIfValid(AddAsyncIfValid, model);

    protected virtual async Task<IActionResult> AddAsyncIfValid(TModel model)
    {
        await dbService.AddModelAsync(model);
        return RedirectToBack();
    }


    [HttpGet("edit")]
    public virtual Task<IActionResult> EditAsync(TId modelId)
    {
        dbService.DeleteModelById(modelId);
        return GetActionResultAsync(View());
    }

    [HttpPost("edit")]
    public Task<IActionResult> EditAsync(TModel model)
        => DoActionIfValid(EditAsyncIfValid, model);

    protected virtual Task<IActionResult> EditAsyncIfValid(TModel model)
    {
        dbService.EditModel(model);
        return GetActionResultAsync(View());
    }


    [HttpDelete("delete")]
    public virtual Task<IActionResult> DeleteAsync(TId modelId)
    {
        dbService.DeleteModelById(modelId);
        return GetActionResultAsync(View());
    }


    Task<IActionResult> DoActionIfValid(Func<TModel, Task<IActionResult>> action, TModel model)
        => ModelState.IsValid ? action(model) : GetActionResultAsync(View(model));

    protected Task<IActionResult> GetActionResultAsync(IActionResult actionResult)
        => Task.FromResult(actionResult);
}