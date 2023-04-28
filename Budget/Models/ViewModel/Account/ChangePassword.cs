using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Budget.Models.ViewModel.Account;

public class ChangePassword
{
    [Display(Name = "Old password")]
    [DataType(DataType.Password)]
    public string? OldPassword { get; set; }

    [Display(Name = "New password")]
    [Required(ErrorMessage = "Enter new password")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = null!;

    [Display(Name = "Repeat new password")]
    [Required(ErrorMessage = "Repeat new password")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Passwords don't match")]
    public string ConfirmNewPassword { get; set; } = null!;
}
