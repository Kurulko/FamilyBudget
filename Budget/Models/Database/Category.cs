using System.Collections;
using System.Collections.Generic;

namespace Budget.Models.Database;

public class Category : DbModel
{
    public string Name { get; set; } = null!;

    public IEnumerable<Operation>? Operations { get; set; }
    public IEnumerable<Category>? SubCategories { get; set; }
}
