using Budget.Models.Database;
using Budget.Services.Db.Users;
using Budget.Services.Db;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Budget.Controllers;

public class OperationController : EditController<Operation, long>
{
    public OperationController(AbsUserService userService, DbModelService<Operation> dbService) : base(userService, dbService) { }

    public override Task<IActionResult> AddAsync()
        => GetActionResultAsync(View());

    protected override async Task<IActionResult> AddAsyncIfValid(Operation model)
    {
        await dbService.AddModelAsync(model);
        return RedirectToHome();
    }

    public override Task<IActionResult> DeleteAsync(long modelId)
    {
        dbService.DeleteModelById(modelId);
        return GetActionResultAsync(View());
    }

    public override Task<IActionResult> EditAsync(long modelId)
        => GetActionResultAsync(View());

    protected override Task<IActionResult> EditAsyncIfValid(Operation model)
    {
        dbService.EditModel(model);
        return GetActionResultAsync(View());
    }
}
