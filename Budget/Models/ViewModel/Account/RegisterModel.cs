using Budget.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Budget.Models.ViewModel.Account;

public class RegisterModel : AccountModel
{
    [RequiredEnter]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;


    [RequiredEnter("Repeat {0}")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The passwords doesn't match")]
    public string ConfirmPassword { get; set; } = null!;
}
