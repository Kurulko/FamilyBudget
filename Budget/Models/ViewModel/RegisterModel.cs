using Budget.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Budget.Models.ViewModel;

public class RegisterModel
{
    [RequiredEnter]
    public string Name { get; set; }

    [RequiredEnter]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [RequiredEnter]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [RequiredEnter("Repeat {0}")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The passwords doesn't match")]
    public string ConfirmPassword { get; set; }

    [Display(Name = "Remember me?")]
    public bool IsRememberMe { get; set; }
}
