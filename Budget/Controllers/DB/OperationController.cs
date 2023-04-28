using Budget.Models.Database;
using Budget.Services.Db.Users;
using Budget.Services.Db;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Budget.Models.ViewModel;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Budget.Services.Db.Operations;
using Budget.Services.Db.Categories;
using Budget.Extensions;
using static Budget.Services.Db.Operations.OperationManager;

namespace Budget.Controllers.DB;

[Route(name)]
public class OperationController : DbModelEditController<Operation>
{
    readonly CategoryService categoryService;
    readonly Service<Currency, long> currencyService;
    public OperationController(UserService userService, OperationService operationService, CategoryService categoryService, Service<Currency, long> currencyService)
        : base(userService, operationService)
        => (this.categoryService, this.currencyService) = (categoryService, currencyService);

    protected override IActionResult RedirectToBack()
        => Redirect($"/{name}/{typeOfOperation.ToStringAndLower()}/{pathToModels}");

    internal const string name = "operations";
    internal const string urlOfModels = "{type}/" + pathToModels;

    protected override Operation CreateAddModel()
    {
        Operation o = base.CreateAddModel();
        o.Money = new();
        o.TypeOfOperation = typeOfOperation;
        return o;
    }

    TypeOfOperation typeOfOperation = new();

    async Task SetData(string? userId = null, long? id = null)
    {
        categoryService.UserId = GetUserId(userId);
        ViewBag.Categories = new SelectList(await categoryService.GetParentCategoriesAsync(), nameof(Category.Id), nameof(Category.Name));

        IEnumerable<Category>? subCategories = null;
        if(id != 0 && id is not null)
        {
            SetUserId(userId);
            Operation? operation = await service.GetModelByIdAsync(id!.Value);
            if(operation is not null)
                subCategories = await categoryService.GetChildCategoriesByParentIdAsync(operation!.CategoryId!.Value);
        }
        ViewBag.SubCategories = (subCategories?.Any() ?? false) ? new SelectList(subCategories, nameof(Category.Id), nameof(Category.Name)) : null;

        ViewBag.Currencies = new SelectList(await currencyService.GetModelsAsync(), nameof(Currency.Id), nameof(Currency.Symbol));
    }

    

    [HttpGet($"{urlOfModels}/{partPathToId}")]
    [HttpGet($"{partPathToUserId}/{urlOfModels}/{partPathToId}")]
    public async Task<IActionResult> GetModelByIdAsync(string? userId, long id, TypeOfOperation type)
    {
        SetTypeOfOperation(type);
        await SetData(userId, id);
        return await base.GetModelByIdAsync(userId, id);
    }


    [HttpGet($"{urlOfModels}/{partPathAdd}")]
    [HttpGet($"{partPathToUserId}/{urlOfModels}/{partPathAdd}")]
    public async Task<IActionResult> AddAsync(string? userId, TypeOfOperation type)
    {
        SetTypeOfOperation(type);
        await SetData(userId);
        return await base.AddAsync(userId);
    }


    [HttpPost($"{urlOfModels}/{partPathAdd}")]
    [HttpPost($"{partPathToUserId}/{urlOfModels}/{partPathAdd}")]
    public async Task<IActionResult> AddAsync(string? userId, Operation model, TypeOfOperation type)
    {
        SetTypeOfOperation(type);
        await SetData(userId);
        return await base.AddAsync(userId, model);
    }


    [HttpGet($"{urlOfModels}/{partPathToId}/{partPathEdit}")]
    [HttpGet($"{partPathToUserId}/{urlOfModels}/{partPathToId}/{partPathEdit}")]
    public async Task<IActionResult> EditAsync(string? userId, long id, TypeOfOperation type)
    {
        SetTypeOfOperation(type);
        await SetData(userId, id);
        return await base.EditAsync(userId, id);
    }


    [HttpPost($"{urlOfModels}/{partPathEdit}")]
    [HttpPost($"{partPathToUserId}/{urlOfModels}/{partPathEdit}")]
    public async Task<IActionResult> EditAsync(string? userId, Operation model, TypeOfOperation type)
    {
        SetTypeOfOperation(type);
        await SetData(userId);
        return await base.EditAsync(userId, model);
    }


    [HttpGet(urlOfModels)]
    [HttpGet($"{partPathToUserId}/{urlOfModels}")]
    public async Task<IActionResult> GetModelsAsync(string? userId, TypeOfOperation type)
    {
        SetUserId(userId);

        OperationService operationService = (service as OperationService)!;
        var groupsMoney = operationService.GetCurrentSumsOfMoney();
        var groupsOperation = await operationService.GetGroupsOperationByPredicateAsync(o => o.TypeOfOperation == type);

        return View(viewModelsName, new ModelWithTypeOfOperation<GroupsOperationAndGroupsMoney>(new GroupsOperationAndGroupsMoney() { GroupsMoney = groupsMoney, GroupsOperation = groupsOperation }, type));
    }


    internal const string pathToCompare = "compare";
    internal const string viewCompareName = "Compare";

    [HttpGet(pathToCompare)]
    [HttpGet($"{partPathToUserId}/{pathToCompare}")]
    public async Task<IActionResult> CompareAsync(string? userId, [FromServices] Service<Currency, long> currencyService)
    {
        ViewBag.Currencies = await currencyService.GetModelsAsync();

        SetUserId(userId);

        OperationService operationService = (service as OperationService)!;

        var groupsMoney = operationService.GetCurrentSumsOfMoneyForMonth();

        return View(viewCompareName, groupsMoney);
    }


    [HttpGet($"{urlOfModels}/{partPathToId}/{partPathDelete}")]
    [HttpGet($"{partPathToUserId}/{urlOfModels}/{partPathToId}/{partPathDelete}")]
    public Task<IActionResult> DeleteAsync(string? userId, long id, TypeOfOperation type)
    {
        SetTypeOfOperation(type);
        return base.DeleteAsync(userId, id);
    }


    void SetTypeOfOperation(TypeOfOperation typeOfOperation)
        => this.typeOfOperation = typeOfOperation;
}
