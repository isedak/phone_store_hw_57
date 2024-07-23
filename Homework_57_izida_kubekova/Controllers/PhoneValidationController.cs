using System;
using System.Globalization;
using System.Linq;
using Homework_57_izida_kubekova.Models.Data;
using Homework_57_izida_kubekova.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Homework_57_izida_kubekova.Controllers
{
    public class PhoneValidationController : Controller
    {
        private readonly StoreContext _db;

        public PhoneValidationController(StoreContext db)
        {
            _db = db;
        }
        
        public bool CheckName(AddAndEditViewModel model)
        {
            var phones = _db.Phones.AsQueryable();
            if (model.Phone.Id is > 0)
                phones = phones.Where(p => p.Id != model.Phone.Id);
            if (string.IsNullOrWhiteSpace(model.Phone.Name))
                return false;
            return !phones.Any(b => b.Name.ToLower() == model.Phone.Name.ToLower());
        }
    }
}