using EduTech.Data;
using EduTech.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduTech.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // 1. KAYIT OL (GET)
        public IActionResult Register()
        {
            return View();
        }

        // 1. KAYIT OL (POST)
        [HttpPost]
        public IActionResult Register(User user)
        {
            // Basit bir kontrol: Email zaten var mı?
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                ViewBag.Error = "Bu email adresi zaten kayıtlı!";
                return View(user);
            }

            // İlk kaydolan kişiyi 'Admin' yapalım, diğerleri 'Student' olsun (Test kolaylığı için)
            if (!_context.Users.Any())
            {
                user.Role = "Admin";
            }
            else
            {
                user.Role = "Student"; // Varsayılan rol
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Kayıt başarılı! Lütfen giriş yapın.";
            return RedirectToAction("Login");
        }

        // 2. GİRİŞ YAP (GET)
        public IActionResult Login()
        {
            return View();
        }

        // 2. GİRİŞ YAP (POST)
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Fullname", user.FullName);
                HttpContext.Session.SetString("Role", user.Role); // Admin mi Student mı?

                TempData["SuccessMessage"] = $"Hoşgeldiniz, {user.FullName}!";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Email veya şifre hatalı!";
            return View();
        }

        // 3. ÇIKIŞ YAP
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}