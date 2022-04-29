using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget.Models
{
    public class OperationsWithMoney
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите кол-во денег")]
        [Display(Name = "Сумма")]
        public decimal Money { get; set; }


        [Display(Name = "Наличными?")]
        public bool IsCash { get; set; }

        [Display(Name = "Время")]
        [DataType(DataType.Date)]
        public DateTime Time { get; set; }

        public string PersonId { get; set; }
        public User Person { get; set; }
    }
}
