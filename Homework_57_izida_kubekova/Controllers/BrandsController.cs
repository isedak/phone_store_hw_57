using System.Linq;
using Homework_57_izida_kubekova.Models;
using Homework_57_izida_kubekova.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Homework_57_izida_kubekova.Controllers
{
    [Authorize]
    public class BrandsController : Controller
    {
        private readonly StoreContext _db;

        public BrandsController(StoreContext db)
        {
            _db = db;
        }
        
        public IActionResult Index()
        {
            var brands = _db.Brands.ToList();
            return View(brands);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RedirectToBrand(int id)
        {
            var brand = _db.Brands.FirstOrDefault(b => b.Id == id);
            if (brand is null)
                return NotFound();
            return Redirect($"{brand.PagePath}");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Add(Brand brand)
        {
            if (brand is null)
                return NotFound();
            if (ModelState.IsValid)
            {
                _db.Brands.Add(brand);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(brand);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            var brand = _db.Brands.FirstOrDefault(p => p.Id == id);
            if (brand is null)
                return NotFound();
            return View(brand);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(Brand brand)
        {
            if (brand is null)
                return NotFound();
            if (ModelState.IsValid)
            {
                _db.Brands.Update(brand);
                _db.SaveChanges();
                return RedirectToAction("Index", new {id = brand.Id});
            }

            return View(brand);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            var brand = _db.Brands.FirstOrDefault(p => p.Id == id);
            if (brand is null)
                return NotFound();
            if (_db.Phones.Any(p => p.BrandId == id))
            {
                ViewBag.BrandDeletionError = "Невозможно удалить бренд, пока не удалены все его товары";
                var brands = _db.Brands.ToList();
                return View("Index", brands);
            }

            return View(brand);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult ConfirmDelete(int? id)
        {
            if (id != null)
            {
                var brand = _db.Brands.FirstOrDefault(p => p.Id == id);
                if (brand != null)
                {
                    _db.Brands.Remove(brand);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}