using System.ComponentModel.DataAnnotations;

namespace Budget.Models.ViewModel
{
    public class LoginModel
    {
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Введите свое имя")]
        public string Name { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Ввидите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }
}
