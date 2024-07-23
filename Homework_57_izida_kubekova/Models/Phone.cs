using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Homework_57_izida_kubekova.Models
{
    public class Phone
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "Введите название от 2 до 35 символов")]
        [Remote("CheckName", "PhoneValidation", AdditionalFields = "Id", ErrorMessage = "Такое название уже есть")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [Range(0.10, 100000000, ErrorMessage = "Цена должна быть в пределах от 0.10 до 100000000")]
        public double Price { get; set; }

        public string Description { get; set; }
        public string ImagePath { get; set; }
        [Required(ErrorMessage = "Вы не выбрали бренд")]
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public bool IsActive { get; set; } = true;

        public double AverageRate { get; set; }
    }
}