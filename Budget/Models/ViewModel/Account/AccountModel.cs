using Budget.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Budget.Models.ViewModel.Account;

public class AccountModel
{
    [RequiredEnter]
    public string Name { get; set; } = null!;

    [RequiredEnter]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }

}
