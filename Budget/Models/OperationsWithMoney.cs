using Budget.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget.Models;

public class OperationsWithMoney
{
    public int Id { get; set; }

    [RequiredEnter]
    public string Name { get; set; }

    [RequiredEnter("sum of money")]
    [Display(Name = "Sum of money")]
    public decimal Money { get; set; }


    [Display(Name = "Is cash?")]
    public bool IsCash { get; set; }

    [DataType(DataType.Date)]
    public DateTime Time { get; set; }

    public string PersonId { get; set; }
    public User Person { get; set; }
}
