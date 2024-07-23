using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Homework_57_izida_kubekova.Models;
using Homework_57_izida_kubekova.Models.Data;
using Homework_57_izida_kubekova.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Homework_57_izida_kubekova.Controllers
{
    public class AccountController : Controller
    {
        private readonly StoreContext _db;

        public AccountController(StoreContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "admin")]
        public IActionResult Index(int? id)
        {
            var users = _db.Users.Include(u => u.Role).ToList();
            if (id.HasValue)
                users = _db.Users.Where(u => u.Id == id.Value).ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == model.Email &&
                    u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user);

                    return RedirectToAction("Index", "Phones");
                }

                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }

            return View(model);
        }

        [NonAction]
        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(
                claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(id),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(10)
                }
            );
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email.ToLower());
                if (user == null)
                {
                    user = new User
                    {
                        Email = model.Email,
                        Password = model.Password,
                        Name = model.Name,
                        RoleId = 2,
                    };
                    Role role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == "user");
                    if (role != null)
                        user.Role = role;
                    _db.Users.Add(user);
                    await _db.SaveChangesAsync();
                    await Authenticate(user);
                    return RedirectToAction("Index", "Phones");
                }

                ModelState.AddModelError("", "Пользователь с таким Email уже существует");
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Phones");
        }

        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            var user = _db.Users.Include(u => u.Role).FirstOrDefault(p => p.Id == id);
            if (user is null)
                return NotFound();
            var model = new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                Roles = _db.Roles.ToList(),
                RoleId = user.RoleId,
                Name = user.Name,
                Password = user.Password,
                ConfirmPassword = user.Password
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(EditUserViewModel model)
        {
            if (model is null)
                return NotFound();
            Role userRole = _db.Roles.FirstOrDefault(r => r.Id == model.RoleId);
            var user = new User
            {
                Id = model.Id,
                Email = model.Email,
                Name = model.Name,
                Password = model.Password,
                RoleId = model.RoleId,
                Role = userRole
            };
            if (ModelState.IsValid)
            {
                _db.Users.Update(user);
                _db.SaveChanges();
                return RedirectToAction("Index", new {id = user.Id});
            }

            return View(model);
        }
    }
}