using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Homework_57_izida_kubekova.Models;
using Homework_57_izida_kubekova.Models.Data;
using Homework_57_izida_kubekova.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Homework_57_izida_kubekova.Controllers
{
    public class PhonesController : Controller
    {
        private readonly IHostEnvironment _appEnv;
        private readonly StoreContext _db;

        public PhonesController(IHostEnvironment appEnv, StoreContext db)
        {
            _appEnv = appEnv;
            _db = db;
        }

        public IActionResult Index()
        {
            var phones = _db.Phones.Where(p => p.IsActive).Include(p => p.Brand).ToList();
            foreach (var phone in phones)
            {
                phone.AverageRate = CountAverageRate(phone.Id);
            }

            _db.SaveChanges();
            return View(phones);
        }

        [Authorize]
        public IActionResult Download(int id)
        {
            var phone = _db.Phones.FirstOrDefault(p => p.Id == id);
            if (phone is null)
                return NotFound();
            string fileName = phone.Name.Replace(" ", "_");
            fileName = $"{fileName}.txt";
            string path = Path.Combine(_appEnv.ContentRootPath, $"Files/{fileName}");
            if (System.IO.File.Exists(path))
            {
                string filetype = "application/txt";
                return PhysicalFile(path, filetype, fileName);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            var phone = _db.Phones.FirstOrDefault(p => p.Id == id);
            if (phone is null)
                return NotFound();
            return View(phone);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult ConfirmDelete(int id)
        {
            var phone = _db.Phones.FirstOrDefault(p => p.Id == id);
            if (phone is null)
                return NotFound();
            phone.IsActive = false;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public IActionResult ShowCustomErrors()
        {
            ViewBag.Message = "Список брендов пуст, внесите хотя бы один";
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult Add()
        {
            if (_db.Brands.ToList().Count == 0)
            {
                return RedirectToAction("ShowCustomErrors");
            }

            var model = new AddAndEditViewModel(new Phone(), _db.Brands.ToList());
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Add(Phone phone)
        {
            AddAndEditViewModel model;
            if (ModelState.IsValid)
            {
                phone.ImagePath ??= "img/no-img.png";
                phone.Description ??= "Описания пока нет";
                phone.AverageRate = CountAverageRate(phone.Id);
                _db.Phones.Add(phone);
                _db.SaveChanges();
                return RedirectToAction("Details", phone);
            }

            model = new AddAndEditViewModel(phone, _db.Brands.ToList());
            return View("Add", model);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            var phone = _db.Phones.Include(p => p.Brand).FirstOrDefault(p => p.Id == id);
            if (phone is null)
                return NotFound();
            var model = new AddAndEditViewModel(phone, _db.Brands.ToList());
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(AddAndEditViewModel model)
        {
            var phone = model.Phone;
            if (phone is null)
                return NotFound();
            if (ModelState.IsValid)
            {
                phone.ImagePath ??= "img/no-img.png";
                phone.AverageRate = CountAverageRate(phone.Id);
                _db.Phones.Update(phone);
                _db.SaveChanges();
                return RedirectToAction("Details", new {phone.Id});
            }

            model = new AddAndEditViewModel(phone, _db.Brands.ToList());
            return View("Edit", model);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var phone = _db.Phones.Include(p => p.Brand).FirstOrDefault(p => p.Id == id);
            if (phone is null)
                return NotFound();
            var reviews = _db.CustomerReviews.Include(r => r.User).Include(r => r.Phone)
                .Where(r => r.PhoneId == id).ToList();
            if (Program.Currencies.Count != 0)
                ViewData["Currencies"] = Program.Currencies;
            phone.AverageRate = CountAverageRate(phone.Id);
            _db.SaveChanges();
            DetailsViewModel model = new DetailsViewModel(phone, reviews);
            return View(model);
        }

        [NonAction]
        private double CountAverageRate(int id)
        {
            var reviews = _db.CustomerReviews.Where(r => r.PhoneId == id).ToList();
            if (reviews.Count == 0)
                return 0;
            var sum = reviews.Sum(r => r.Rate);
            if (sum == 0)
                return 0;
            double averageRate = Math.Round(((double) sum / reviews.Count), 1, MidpointRounding.ToEven);
            return averageRate;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}