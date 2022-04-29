using System.ComponentModel.DataAnnotations;

namespace Budget.Models.ViewModel
{
    public class RegisterModel
    {
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Введите свое имя")]
        public string Name { get; set; }

        [Display(Name = "Почта")]
        [Required(ErrorMessage = "Введите свою почту")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Придумайте пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Повторный пароль")]
        [Required(ErrorMessage = "Повторите пароль")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }
}
