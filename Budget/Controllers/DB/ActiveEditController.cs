using Budget.Services.Db.Users;
using Budget.Services.Db;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Controllers.DB;

public abstract class ActiveEditController<TModel, TId> : EditController<TModel, TId> where TModel : class, new()
{
    public ActiveEditController(UserService userService, Service<TModel, TId> service) : base(userService, service) { }

    [HttpGet(pathToModelById)]
    public virtual new Task<IActionResult> GetModelByIdAsync(TId id)
        => base.GetModelByIdAsync(id);


    [HttpGet(pathToAddModel)]
    public virtual new Task<IActionResult> AddAsync()
        => base.AddAsync();


    [HttpGet(pathToEditModelGet)]
    public virtual new Task<IActionResult> EditAsync(TId id)
        => base.EditAsync(id);


    [HttpGet(pathToModels)]
    public virtual new Task<IActionResult> GetModelsAsync()
        => base.GetModelsAsync();


    [HttpGet(pathToDeleteModel)]
    public virtual new Task<IActionResult> DeleteAsync(TId id)
        => base.DeleteAsync(id);


    [HttpPost(pathToAddModel)]
    public virtual new Task<IActionResult> AddAsync(TModel model)
        => base.AddAsync(model);


    [HttpPost(pathToEditModelPost)]
    public virtual new Task<IActionResult> EditAsync(TModel model)
        => base.EditAsync(model);
}
