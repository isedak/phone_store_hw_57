using System;
using System.Linq;
using System.Threading.Tasks;
using Homework_57_izida_kubekova.Models;
using Homework_57_izida_kubekova.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Homework_57_izida_kubekova.Controllers
{
    public class CustomerReviewsController : Controller
    {
        private readonly StoreContext _db;

        public CustomerReviewsController(StoreContext db)
        {
            _db = db;
        }
        
        [Authorize(Roles = "admin, user")]
        public IActionResult Index(int? phoneId)
        {
            var reviews = _db.CustomerReviews.Include(r => r.Phone).Include(r => r.User).ToList();
            if (phoneId.HasValue)
                reviews = _db.CustomerReviews.Where(r => r.PhoneId == phoneId.Value).ToList();
            return View(reviews);
        }

        [Authorize(Roles = "user")]
        public IActionResult Add(int phoneId, string userEmail)
        {
            var phone = _db.Phones.Include(p => p.Brand).FirstOrDefault(p => p.Id == phoneId);
            var user = _db.Users.Include(p => p.Role).FirstOrDefault(u => u.Email == userEmail);
            if (phone is null)
                return NotFound();
            if (user is null)
                return NotFound();
            CustomerReview customerReview = new CustomerReview()
            {
                Phone = phone,
                User = user,
                UserId = user.Id,
                PhoneId = phone.Id
            };

            return View(customerReview);
        }

        [Authorize(Roles = "user")]
        [HttpPost]
        public IActionResult Add(CustomerReview customerReview)
        {
            if (customerReview is null)
                return NotFound();
            if (ModelState.IsValid)
            {
                customerReview.Date = DateTime.Now;
                _db.CustomerReviews.Add(customerReview);
                _db.SaveChanges();
                return RedirectToAction("Index", customerReview.PhoneId);
            }

            return View(customerReview);
        }
        
        [Authorize(Roles = "admin")]
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            var review = await _db.CustomerReviews.Include(r => r.Phone).Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (review is null)
                return NotFound();
            return View(review);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                var review = new CustomerReview {Id = id.Value};
                _db.Entry(review).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}