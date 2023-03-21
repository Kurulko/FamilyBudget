using Budget.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Budget.Models.ViewModel;

public class LoginModel
{
    [RequiredEnter]
    public string Name { get; set; }

    [RequiredEnter]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}
