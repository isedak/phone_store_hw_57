using Homework_57_izida_kubekova.Models.Data;
using Homework_57_izida_kubekova.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Homework_57_izida_kubekova.Controllers
{
    public class UserEditValidationController : Controller
    {
        private readonly StoreContext _db;

        public UserEditValidationController(StoreContext db)
        {
            _db = db;
        }

        public bool CheckInput(EditUserViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                return false;
            return true;
        }

        public bool CheckPassword(EditUserViewModel model)
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
    }
}