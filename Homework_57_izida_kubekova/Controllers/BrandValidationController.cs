using System;
using System.Globalization;
using System.Linq;
using Homework_57_izida_kubekova.Models.Data;
using Microsoft.AspNetCore.Mvc;

namespace Homework_57_izida_kubekova.Controllers
{
    public class BrandValidationController : Controller
    {
        private readonly StoreContext _db;

        public BrandValidationController(StoreContext db)
        {
            _db = db;
        }

        public bool CheckName(string name, int? id)
        {
            var brands = _db.Brands.AsQueryable();
            if (id is > 0)
                brands = brands.Where(p => p.Id != id);
            if (string.IsNullOrWhiteSpace(name))
                return false;
            return !brands.Any(b => b.Name.ToLower() == name.ToLower());
        }

        public bool CheckDate(string dateOfFoundation)
        {
            
            if (DateTime.TryParse(dateOfFoundation, out var date))
                return date >= DateTime.Today.AddYears(-100) && date <= DateTime.Today;
            return false;
        }
    }
}