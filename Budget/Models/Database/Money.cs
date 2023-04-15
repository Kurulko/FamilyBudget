using System.ComponentModel.DataAnnotations;

namespace Budget.Models.Database;

public class Money : DbModel
{
    public decimal Count { get; set; }
    [Display(Name = "Type")]
    public TypeOfMoney TypeOfMoney { get; set; }

    public long? CurrencyId { get; set; }
    public Currency? Currency { get; set; }
    public Operation? Operation { get; set; }
}
public enum TypeOfMoney
{
    Card, Cash
}