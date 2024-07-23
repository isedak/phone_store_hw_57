using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Homework_57_izida_kubekova.Models
{
    public class CustomerReview
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [Remote("CheckInput", "ReviewValidation", ErrorMessage = "Вы не написали отзыв")]
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public string Review { get; set; }

        [Required(ErrorMessage = "Вы не поставили оценку")]
        [Range(0, 5, ErrorMessage = "Оценка должна быть в переделах от 0 до 5")]
        public int Rate { get; set; }

        [BindProperty, DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public int PhoneId { get; set; }
        public Phone Phone { get; set; }
    }
}