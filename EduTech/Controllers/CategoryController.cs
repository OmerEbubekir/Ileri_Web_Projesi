using EduTech.Data;
using EduTech.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduTech.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // 1. LİSTELEME
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // 2. EKLEME (GET)
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("Index");
            return View();
        }

        // 2. EKLEME (POST)
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Kategori eklendi!";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // 3. DÜZENLEME (GET)
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("Index");

            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();

            return View(category);
        }

        // 3. DÜZENLEME (POST)
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Kategori güncellendi!";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // 4. SİLME (GET - Direkt siler)
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("Index");

            var category = _context.Categories.Find(id);
            if (category != null)
            {
                // İlişkili kurslar varsa hata verebilir, şimdilik basit silme yapıyoruz
                _context.Categories.Remove(category);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Kategori silindi!";
            }
            return RedirectToAction("Index");
        }
    }
}