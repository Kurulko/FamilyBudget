using Budget.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Budget.Models;

public class MoneyForEveryone
{
    public int Id { get; set; }

    [RequiredEnter("sum of money")]
    [Display(Name = "Sum of money")]
    public decimal Paid { get; set; }


    public decimal MiddleValue { get; set; }
    public decimal MustGet { get; set; }
    public decimal MustPay { get; set; }

    public string PersonId { get; set; }
    public User Person { get; set; }
}
