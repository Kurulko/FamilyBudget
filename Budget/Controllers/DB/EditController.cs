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
using System.Collections;
using System.Collections.Generic;

namespace Budget.Controllers.DB;

[Authorize]
public abstract class EditController<TModel, TId> : BudgetController where TModel : class, new()
{
    protected readonly Service<TModel, TId> service;
    public EditController(UserService userService, Service<TModel, TId> service) : base(userService)
        => this.service = service;

    internal protected const string viewModelName = "Model";
    internal protected const string viewModelsName = "Models";


    internal protected const string partPathToId = "{id}";
    internal protected const string partPathAdd = "add";
    internal protected const string partPathEdit = "edit";
    internal protected const string partPathDelete = "delete";

    #region Actions

    internal protected const string pathToModels = "models";

    protected virtual async Task<IEnumerable<TModel>> ModelsAsync()
            => await service.GetModelsAsync();

    protected virtual async Task<IActionResult> GetModelsAsync()
            => View(viewModelsName, await ModelsAsync());


    internal protected const string pathToModelById = $"{pathToModels}/{partPathToId}";


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

    protected IActionResult RedirectToBackIfModelIsNull<T>(IActionResult action, T? model)
        => model is not null ? action : RedirectToBack();
    protected Task<IActionResult> DoActionIfValid<T>(Func<T, Task<IActionResult>> action, ModelWithMode<T> modelWithMode, string viewName = viewModelName)
        => ModelState.IsValid ? action(modelWithMode.Model!) : GetActionResultAsync(View(viewName, modelWithMode));

    protected Task<IActionResult> GetActionResultAsync(IActionResult actionResult)
        => Task.FromResult(actionResult);
    protected IActionResult ViewModel<T>(T model, Mode mode, string viewName = viewModelName)
        => View(viewName, new ModelWithMode<T>(model, mode));

    #endregion
}