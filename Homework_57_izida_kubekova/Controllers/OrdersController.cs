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
    public class OrdersController : Controller
    {
        private readonly StoreContext _db;

        public OrdersController(StoreContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "admin, user")]
        public IActionResult Index(int? userId)
        {
            var orders = _db.Orders.Include(o => o.Phone).Include(o => o.User).ToList();
            if (userId.HasValue)
            {
                orders = _db.Orders.Where(o => o.User.Id == userId).ToList();
            }

            return View(orders);
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
            Order order = new Order()
            {
                Phone = phone,
                User = user,
                UserId = user.Id,
                PhoneId = phone.Id
            };
            return View(order);
        }

        [Authorize(Roles = "user")]
        [HttpPost]
        public IActionResult Add(Order order)
        {
            if (order is null)
                return NotFound();
            if (ModelState.IsValid)
            {
                order.Date = DateTime.Now;
                _db.Orders.Add(order);
                _db.SaveChanges();
                return RedirectToAction("Index", order.UserId);
            }

            return View(order);
        }

        [Authorize(Roles = "user")]
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            var order = await _db.Orders.Include(o => o.Phone).FirstOrDefaultAsync(p => p.Id == id);
            if (order is null)
                return NotFound();
            return View(order);
        }

        [Authorize(Roles = "user")]
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                var order = new Order {Id = id.Value};
                _db.Entry(order).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Phones");
            }

            return NotFound();
        }
    }
}