using Budget.Models.Database;
using System.Collections.Generic;

namespace Budget.Models.ViewModel;

public class GroupOperation
{
    public int Month { get; set; }
    public int Year { get; set; }
    public IEnumerable<Operation> Operations { get; set; } = null!;
}
