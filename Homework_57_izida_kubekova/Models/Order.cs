using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Homework_57_izida_kubekova.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Remote("CheckAddress", "OrderValidation", ErrorMessage = "Введите адрес доставки")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Remote("CheckContact", "OrderValidation", ErrorMessage = "Поле обязательно для заполнения")]
        public string ContactPhone { get; set; }

        public int PhoneId { get; set; }
        public Phone Phone { get; set; }
        [BindProperty, DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
    }
}