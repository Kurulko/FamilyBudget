using Budget.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace Budget.Services.Db;

public class CategoryService : DbModelService<Category>
{
    public CategoryService(BudgetContext db) : base(db) { }

    protected override DbSet<Category> models => db.Categories;
}
