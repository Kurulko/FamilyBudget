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

namespace Budget.Controllers.DB;

[Route("operations")]
public class OperationController : DbModelEditController<Operation>
{
    readonly DbModelService<Category> categoryService;
    readonly DbModelService<Currency> currencyService;
    public OperationController(AbsUserService userService, DbModelService<Operation> dbService, DbModelService<Category> categoryService, DbModelService<Currency> currencyService)
        : base(userService, dbService)
        => (this.categoryService, this.currencyService) = (categoryService, currencyService);

    protected override IActionResult RedirectToBack()
        => Redirect("/operations/models");

    const string urlOfModels = "{type}/models";

    async Task SetData()
    {
        ViewBag.Categories = new SelectList(await categoryService.GetAllModelsAsync(), "Id", "Name");
        ViewBag.Currencies = new SelectList(await currencyService.GetAllModelsAsync(), "Id", "ShortName");
    }

    [HttpGet(urlOfModels + "/{id:long}")]
    public override async Task<IActionResult> GetModelByIdAsync(long id)
    {
        await SetData();
        return await base.GetModelByIdAsync(id);
    }

    [HttpGet(urlOfModels + "/add")]
    public override async Task<IActionResult> AddAsync()
    {
        await SetData();
        return await base.AddAsync();
    }

    [HttpGet(urlOfModels + "/{id}/edit")]
    public override async Task<IActionResult> EditAsync(long id)
    {
        await SetData();
        return await base.EditAsync(id);
    }

    [HttpGet(urlOfModels)]
    public override async Task<IActionResult> GetModelsAsync()
    {
        TypeOfOperation typeOfOperation = GetTypeOfOperationFromQuery();

        SetUserId();

        var groupsMoney = (service as OperationService)!.GetCurrentSumsOfMoney();
        var operations = await service.GetModelsByPredicateAsync(o => o.TypeOfOperation == typeOfOperation);

        var groupsOperation = operations.GroupBy(o => new MonthAndYear(o.DateTime.Month, o.DateTime.Year),
            (MonthAndYear may, IEnumerable<Operation> ops) => new GroupOperation() { Month = may.Month, Year = may.Year, Operations = ops });

        return View("Models", new ModelWithTypeOfOperation<GroupsOperationAndGroupsMoney>(new GroupsOperationAndGroupsMoney() { GroupsMoney = groupsMoney, GroupsOperation = groupsOperation }, typeOfOperation));
    }
    public record MonthAndYear(int Month, int Year);


    [HttpGet(urlOfModels + "/{id}/delete")]
    public override Task<IActionResult> DeleteAsync(long id)
        => base.DeleteAsync(id);

    [HttpPost(urlOfModels + "/add")]
    public override Task<IActionResult> AddAsync(Operation model)
        => base.AddAsync(model);

    [HttpPost(urlOfModels + "/edit")]
    public override Task<IActionResult> EditAsync(Operation model)
        => base.EditAsync(model);


    TypeOfOperation GetTypeOfOperationFromQuery()
    {
        string? typeStr = HttpContext.GetRouteValue("type")?.ToString();

        TypeOfOperation typeOfOperation = typeStr?.ToLower() switch
        {
            "purchase" => TypeOfOperation.Purchase,
            "receiving" => TypeOfOperation.Receiving,
            _ => TypeOfOperation.Purchase
        };

        return typeOfOperation;
    }
}
