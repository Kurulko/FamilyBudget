using Budget.Models.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Services.Db.Categories;

public class CategoryManager : CategoryService
{
    public CategoryManager(BudgetContext db) : base(db) { }

    protected override DbSet<Category> models => db.Categories;

    public override async Task<IEnumerable<Category>> GetAllChildCategoriesIdAsync()
        => (await GetModelsAsync()).ToList().Where(c => c.ParentCategoryId is not null);

    public override async Task<IEnumerable<Category>> GetChildCategoriesByParentIdAsync(long parentId)
        => (await GetModelsAsync()).ToList().Where(c => c.ParentCategoryId == parentId);

    public override async Task<IEnumerable<Category>> GetParentCategoriesAsync()
        => (await GetModelsAsync()).ToList().Where(c => c.ParentCategoryId is null);
}
