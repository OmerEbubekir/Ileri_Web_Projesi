using System.Diagnostics;
using EduTech.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduTech.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Subscribe(string email)
        {
            // Burada aslýnda veritabanýna kaydetmiyoruz ama kullanýcý öyle sanacak :)
            TempData["SuccessMessage"] = "Bültenimize baþarýyla abone oldunuz! Kampanyalar mail adresinize gönderilecektir.";
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
