using EduTech.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduTech.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Profile()
        {
            // Session'dan verileri alıp sayfaya gönderiyoruz (Madde 7'ye bir örnek daha)
            ViewBag.UserEmail = "user@example.com"; // Gerçekte DB'den çekilir ama Session yeterli
            ViewBag.Role = HttpContext.Session.GetString("Role");
            ViewBag.Name = HttpContext.Session.GetString("Fullname");

            if (ViewBag.Name == null) return RedirectToAction("Login", "Auth");

            return View();
        }

        public IActionResult MyCourses()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            // Giriş yapan kullanıcının satın aldığı kursları getir
            var mySales = _context.Sales
                          .Include(s => s.Course) // Kurs detaylarını da getir
                          .Where(s => s.UserId == userId)
                          .ToList();

            return View(mySales);
        }
    }
}