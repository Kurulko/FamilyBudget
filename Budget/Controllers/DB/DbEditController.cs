﻿using Budget.Models.Database;
using Budget.Services.Db.Users;
using Budget.Services.Db;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Controllers.DB;

public abstract class DbModelEditController<TModel> : EditController<TModel, long> where TModel : DbModel, new()
{
    public DbModelEditController(UserService userService, DbModelService<TModel> dbService) : base(userService, dbService) { }

    protected const string partPathToUserId = "{userId}";

    protected string userId => userService.GetUserIdByClaims(User)!;

    protected virtual void SetUserId(string? userId)
        => (service as DbModelService<TModel>)!.UserId = GetUserId(userId);

    protected string GetUserId(string? userId)
        => userId is null ? this.userId : userId;



    protected virtual Task<IActionResult> GetModelByIdAsync(string? userId, long id)
    {
        SetUserId(userId);
        return base.GetModelByIdAsync(id);
    }

    protected virtual Task<IActionResult> GetModelsAsync(string? userId)
    {
        SetUserId(userId);
        return base.GetModelsAsync();
    }

    protected virtual Task<IActionResult> DeleteAsync(string? userId, long id)
    {
        SetUserId(userId);
        return base.DeleteAsync(id);
    }

    protected virtual Task<IActionResult> EditAsync(string? userId, long id)
    {
        SetUserId(userId);
        return base.EditAsync(id);
    }

    protected virtual Task<IActionResult> EditAsync(string? userId, TModel model)
    {
        SetUserId(userId);
        return base.EditAsync(model);
    }

    protected virtual Task<IActionResult> AddAsync(string? userId)
    {
        SetUserId(userId);
        return base.AddAsync();
    }

    protected virtual Task<IActionResult> AddAsync(string? userId, TModel model)
    {
        SetUserId(userId);
        return base.AddAsync(model);
    }
}
