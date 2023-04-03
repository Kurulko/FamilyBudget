using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget.Models.Database;

public class Category : DbModel
{
    public string Name { get; set; } = null!;

    public IEnumerable<Operation>? Operations { get; set; }


    public long ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public IEnumerable<Category>? ChildCategories { get; set; }
}
