using System.Linq;
using Homework_57_izida_kubekova.Models.Data;
using Homework_57_izida_kubekova.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Homework_57_izida_kubekova.Controllers
{
    public class RegisterValidationController : Controller
    {
        private readonly StoreContext _db;

        public RegisterValidationController(StoreContext db)
        {
            _db = db;
        }

        public bool CheckInput(RegisterViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                return false;
            return true;
        }

        public bool CheckPassword(RegisterViewModel model)
        {
            if (model.Password.Length >= 8)
            {
                if (!model.Password.Equals(model.Password.ToLower()))
                {
                    string password = model.Password;
                    int temp = 0;
                    foreach (char ch in password)
                    {
                        if (char.IsDigit(ch))
                            temp += 1;
                    }

                    if (temp >= 1)
                        return true;
                }

                return false;
            }

            return false;
        }

        public bool CheckEmail(RegisterViewModel model)
        {
            bool isInList = _db.Users.Any(u => u.Email.ToLower() == model.Email.ToLower());
            return isInList != true;
        }
    }
}