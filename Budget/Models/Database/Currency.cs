using System.Collections.Generic;

namespace Budget.Models.Database;

public class Currency : DbModel
{
    public string? ShortName { get; set; }
    public string FullName { get; set; } = null!;

    public IEnumerable<Money>? Money { get; set; }
}
