using System;

namespace Budget.Models.Database;

public class Operation : DbModel
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime DateTime { get; set; }

    public Money? Money { get; set; }
    public Category? Category { get; set; }
}
