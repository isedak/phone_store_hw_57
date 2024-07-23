using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Homework_57_izida_kubekova.Models.ViewModels
{
    public class EditUserViewModel
    {

        public EditUserViewModel()
        {
        }

        public EditUserViewModel(int id, string email, List<Role> roles, int? roleId, string name, string password, string confirmPassword)
        {
            Id = id;
            Email = email;
            Roles = roles;
            RoleId = roleId;
            Name = name;
            Password = password;
            ConfirmPassword = confirmPassword;
        }

        public int Id { get; set; }

        public string Email { get; set; }
        public List<Role> Roles { get; set; }

        [Required(ErrorMessage = "Не указана роль")]
        public int? RoleId { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Remote("CheckInput", "UserEditValidation", ErrorMessage =
            "Не указано имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [Remote("CheckPassword", "UserEditValidation",
            ErrorMessage = "Пароль должен содержать минимум 8 символов, хотя бы одну цифру и одну заглавную букву")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}