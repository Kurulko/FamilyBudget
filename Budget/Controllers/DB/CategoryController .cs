using Budget.Services.Db.Users;
using Budget.Services.Db;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Budget.Models.Database;
using Budget.Services.Db.Categories;
using Budget.Models.ViewModel.Helpers;
using Budget.Models.ViewModel;
using Budget.Services.Db.Operations;

namespace Budget.Controllers.DB;

[Route(name)]
public class CategoryController : ActiveDbModelEditController<Category>
{
    public CategoryController(UserService userService, CategoryService dbService) : base(userService, dbService) { }

    const string name = "categories";
    protected override IActionResult RedirectToBack()
        => Redirect($"/{name}/{pathToModels}");

    const string partPathToParentId = "{parentId}";

    [HttpGet($"{pathToAddModel}/{partPathToParentId}")]
    [HttpGet($"{partPathToUserId}/{pathToAddModel}/{partPathToParentId}")]
    public Task<IActionResult> AddAsync(string? userId, long parentId)
    {
        
        Category category = service.CreateAddModel();
        category.ParentCategoryId = parentId;
        return GetActionResultAsync(ViewModel(category, Mode.Add));
    }
}
