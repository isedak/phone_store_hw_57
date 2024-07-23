using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Homework_57_izida_kubekova.Models
{
    public class Brand
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [StringLength(35, MinimumLength = 2, ErrorMessage = "Введите название от 2 до 35 символов")]
        [Remote("CheckName", "BrandValidation", ErrorMessage = "Такой бренд уже есть",
            AdditionalFields = "Id")]
        public string Name { get; set; }
        
        [Url(ErrorMessage = "Вы ввели некорректную ссылку на сайт")]
        public string PagePath { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [EmailAddress(ErrorMessage = "Вы ввели неправильный электронный адрес")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [Remote("CheckDate", "BrandValidation",
            ErrorMessage = "Вы ввели некорректную дату")]
        [BindProperty, DisplayFormat(DataFormatString = "{0:dd MMM yyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfFoundation { get; set; }
    }
}