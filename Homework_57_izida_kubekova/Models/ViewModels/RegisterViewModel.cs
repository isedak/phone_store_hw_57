using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Homework_57_izida_kubekova.Models.ViewModels
{
    public class RegisterViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Не указан Email")]
        [Display(Name = "Электронный адрес")]
        [Remote("CheckEmail", "RegisterValidation", ErrorMessage = "Такой Email уже зарегистрирован")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Remote("CheckInput", "RegisterValidation", ErrorMessage =
            "Не указано имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [Remote("CheckPassword", "RegisterValidation",
            ErrorMessage = "Пароль должен содержать минимум 8 символов, хотя бы одну цифру и одну заглавную букву")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}