using Budget.Models.Database;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Services.Db.Categories;

public abstract class CategoryService : DbModelService<Category>
{
    public CategoryService(BudgetContext db) : base(db) { }

    public abstract Task<IEnumerable<Category>> GetParentCategoriesAsync();
    public abstract Task<IEnumerable<Category>> GetChildCategoriesByParentIdAsync(long parentId);
    public abstract Task<IEnumerable<Category>> GetAllChildCategoriesIdAsync();
}
