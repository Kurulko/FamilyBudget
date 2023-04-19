using Budget.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Budget.Models.ViewModel.Account;

public class AccountModel
{
    [RequiredEnter(nameof(Name))]
    public string Name { get; set; } = null!;

    [RequiredEnter(nameof(Password))]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Display(Name = "Remember me?")]
    public bool IsRememberMe { get; set; }

}
