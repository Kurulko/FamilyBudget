using Budget.Models.Database;
using Budget.Services.Db.Users;
using Budget.Services.Db;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Controllers.DB;

public class ActiveDbModelEditController<TModel> : DbModelEditController<TModel> where TModel : DbModel, new()
{
    public ActiveDbModelEditController(UserService userService, DbModelService<TModel> dbService) : base(userService, dbService) { }

    [HttpGet(pathToModelById)]
    [HttpGet($"{partPathToUserId}/{pathToModelById}")]
    public virtual new Task<IActionResult> GetModelByIdAsync(string? userId, long id)
    => base.GetModelByIdAsync(userId, id);


    [HttpGet(pathToAddModel)]
    [HttpGet($"{partPathToUserId}/{pathToAddModel}")]
    public virtual new Task<IActionResult> AddAsync(string? userId)
        => base.AddAsync(userId);

    [HttpGet(pathToEditModelGet)]
    [HttpGet($"{partPathToUserId}/{pathToEditModelGet}")]
    public virtual new Task<IActionResult> EditAsync(string? userId, long id)
        => base.EditAsync(userId, id);


    [HttpGet(pathToModels)]
    [HttpGet($"{partPathToUserId}/{pathToModels}")]
    public virtual new Task<IActionResult> GetModelsAsync(string? userId)
        => base.GetModelsAsync(userId);

    [HttpGet(pathToDeleteModel)]
    [HttpGet($"{partPathToUserId}/{pathToDeleteModel}")]
    public virtual new Task<IActionResult> DeleteAsync(string? userId, long id)
        => base.DeleteAsync(userId, id);


    [HttpPost(pathToAddModel)]
    [HttpPost($"{partPathToUserId}/{pathToAddModel}")]
    public virtual new Task<IActionResult> AddAsync(string? userId, TModel model)
        => base.AddAsync(userId, model);


    [HttpPost(pathToEditModelPost)]
    [HttpPost($"{partPathToUserId}/{pathToEditModelPost}")]
    public virtual new Task<IActionResult> EditAsync(string? userId, TModel model)
        => base.EditAsync(userId, model);
}
