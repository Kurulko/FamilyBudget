using System.Collections.Generic;

namespace Budget.Models.Database;

public class Currency
{
    public long Id { get; set; }
    public string FullName { get; set; } = null!;
    public string? ShortName { get; set; }
    public char Symbol { get; set; }

    public IEnumerable<Money>? Money { get; set; }

    public static string Display(char symbol, decimal money)
        => symbol == '$' ? $"{symbol}{money}" : $"{money}{symbol}";
}
