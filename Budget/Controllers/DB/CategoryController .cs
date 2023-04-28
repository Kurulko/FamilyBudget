using Budget.Services.Db.Users;
using Budget.Services.Db;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Budget.Models.Database;
using Budget.Services.Db.Categories;
using Budget.Models.ViewModel.Helpers;
using Budget.Models.ViewModel;
using Budget.Services.Db.Operations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Budget.Controllers.DB;

[Route(name)]
public class CategoryController : ActiveDbModelEditController<Category>
{
    public CategoryController(UserService userService, CategoryService dbService) : base(userService, dbService) { }

    internal const string name = "categories";
    protected override IActionResult RedirectToBack()
        => Redirect($"/{name}/{pathToModels}");

    internal const string partPathToParentId = "{parentId}";

    [HttpGet($"{pathToAddModel}/{partPathToParentId}")]
    [HttpGet($"{partPathToUserId}/{pathToAddModel}/{partPathToParentId}")]
    public Task<IActionResult> AddAsync(string? userId, long parentId)
    {
        SetUserId(userId);
        Category category = service.CreateAddModel();
        category.ParentCategoryId = parentId;
        return GetActionResultAsync(ViewModel(category, Mode.Add));
    }
    protected override async Task<IEnumerable<Category>> ModelsAsync()
        => await GetParentCategoriesAsync();

    public override async Task<IActionResult> EditAsync(string? userId, long id)
    {
        await SetData();
        return await base.EditAsync(userId, id);
    }
    public override async Task<IActionResult> EditAsync(string? userId, Category model)
    {
        await SetData();
        return await base.EditAsync(userId, model);
    }


    async Task SetData()
        => ViewBag.ParentCategories = new SelectList(await GetParentCategoriesAsync(), nameof(Category.Id), nameof(Category.Name));
    async Task<IEnumerable<Category>> GetParentCategoriesAsync()
        => await (service as CategoryService)!.GetParentCategoriesAsync();
}
