using System;
using System.ComponentModel.DataAnnotations;

namespace Budget.Models.Database;

public class Operation : DbModel
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;

    [Display(Name = "Monthly?")]
    public bool IsMonthly { get; set; }

    [Display(Name = "Type")]
    public TypeOfOperation TypeOfOperation { get; set; }

    public long? MoneyId { get; set; }
    public Money? Money { get; set; }
    public long? CategoryId { get; set; }
    public Category? Category { get; set; }
    public long? SubCategoryId { get; set; }
    public Category? SubCategory { get; set; }
}

public enum TypeOfOperation
{
    Purchase, Receiving
}