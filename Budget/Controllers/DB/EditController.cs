using Budget.Services.Db.Users;
using Budget.Services.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Budget.Models.ViewModel;
using Budget.Models.ViewModel.Helpers;
using Budget.Models.Database;

namespace Budget.Controllers.DB;

[Authorize]
public abstract class EditController<TModel, TId> : BudgetController where TModel : class, new()
{
    protected readonly Service<TModel, TId> service;
    public EditController(UserService userService, Service<TModel, TId> service) : base(userService)
        => this.service = service;

    protected const string partPathToId = "{id}";
    protected const string partPathAdd = "add";
    protected const string partPathEdit = "edit";
    protected const string partPathDelete = "delete";

    #region Actions

    protected const string pathToModels = "models";

    protected virtual async Task<IActionResult> GetModelsAsync()
            => View("Models", await service.GetModelsAsync());


    protected const string pathToModelById = $"{pathToModels}/{partPathToId}";

    protected virtual TModel CreateAddModel()
        => service.CreateAddModel();

    protected virtual async Task<IActionResult> GetModelByIdAsync(TId id)
    {
        TModel? model = await service.GetModelByIdAsync(id);

        if (model is null)
            return RedirectToBack();

        return ViewModel(model!, Mode.Get);
    }


    protected const string pathToAddModel = $"{pathToModels}/{partPathAdd}";

    protected virtual Task<IActionResult> AddAsync()
        => GetActionResultAsync(ViewModel(CreateAddModel(), Mode.Add));


    protected virtual Task<IActionResult> AddAsync(TModel model)
        => DoActionIfValid(AddAsyncIfValid, new ModelWithMode<TModel>(model, Mode.Add));

    protected virtual async Task<IActionResult> AddAsyncIfValid(TModel model)
    {
        await service.AddModelAsync(model);
        return RedirectToBack();
    }


    protected const string pathToEditModelGet = $"{pathToModelById}/{partPathEdit}";

    protected virtual async Task<IActionResult> EditAsync(TId id)
    {
        TModel? model = await service.GetModelByIdAsync(id);
        return RedirectToBackIfModelIsNull(ViewModel(model!, Mode.Edit), model);
    }


    protected const string pathToEditModelPost = $"{pathToModels}/{partPathEdit}";

    protected virtual Task<IActionResult> EditAsync(TModel model)
        => DoActionIfValid(EditAsyncIfValid, new ModelWithMode<TModel>(model, Mode.Edit));

    protected virtual Task<IActionResult> EditAsyncIfValid(TModel model)
    {
        service.EditModel(model);
        return GetActionResultAsync(RedirectToBack());
    }


    protected const string pathToDeleteModel = $"{pathToModelById}/{partPathDelete}";

    protected virtual Task<IActionResult> DeleteAsync(TId id)
    {
        service.DeleteModelById(id);
        return GetActionResultAsync(RedirectToBack());
    }

    #endregion

    #region Action Helpers

    protected IActionResult RedirectToBackIfModelIsNull(IActionResult action, TModel? model)
        => model is not null ? action : RedirectToBack();
    protected Task<IActionResult> DoActionIfValid(Func<TModel, Task<IActionResult>> action, ModelWithMode<TModel> modelWithMode)
        => ModelState.IsValid ? action(modelWithMode.Model!) : GetActionResultAsync(View(modelWithMode));

    protected Task<IActionResult> GetActionResultAsync(IActionResult actionResult)
        => Task.FromResult(actionResult);
    protected IActionResult ViewModel(TModel model, Mode mode)
        => View("Model", new ModelWithMode<TModel>(model, mode));

    #endregion
}