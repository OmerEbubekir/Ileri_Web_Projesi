using EduTech.Data;
using EduTech.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduTech.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;

        // Dependency Injection ile veritabanını çağırdık
        public CourseController(AppDbContext context)
        {
            _context = context;
        }

        // 1. ACTION: Listeleme (READ)
        public IActionResult Index(int? categoryId)
        {
            // Veritabanı sorgusunu başlat
            var coursesQuery = _context.Courses.Include(c => c.Category).AsQueryable();

            // Eğer bir kategori ID geldiyse, sadece o kategoriye ait olanları filtrele
            if (categoryId.HasValue)
            {
                coursesQuery = coursesQuery.Where(x => x.CategoryId == categoryId);
            }

            // Sidebar'da kategorileri listelemek için tüm kategorileri View'e taşıyoruz
            ViewBag.Categories = _context.Categories.ToList();

            // Seçili kategoriyi işaretlemek için view'e gönderelim
            ViewBag.SelectedCategoryId = categoryId;

            return View(coursesQuery.ToList());
        }

        // 2. DETAILS METODU (YENİ)
        public IActionResult Details(int id)
        {
            // Kursu ve kategorisini bul
            var course = _context.Courses
                         .Include(c => c.Category)
                         .FirstOrDefault(x => x.Id == id);

            if (course == null) return NotFound();

            return View(course);
        }

        // 2. ACTION: Ekleme Sayfasını Göster (CREATE - GET)
        public IActionResult Create()
        {
            // GÜVENLİK KONTROLÜ: Admin değilse anasayfaya at
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                TempData["Error"] = "Bu işlem için yetkiniz yok!"; // İsterseniz bunu Layout'ta kırmızı toast ile gösterebilirsiniz
                return RedirectToAction("Index", "Home");
            }
            // Dropdown için kategorileri ViewBag ile sayfaya taşıyoruz
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // 3. ACTION: Ekleme İşlemini Yap (CREATE - POST)
        [HttpPost]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(course.ImageUrl))
                {
                    course.ImageUrl = "https://picsum.photos/400/300";
                }

                _context.Courses.Add(course);
                _context.SaveChanges();

                
                TempData["SuccessMessage"] = "Kurs başarıyla oluşturuldu!";
               
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View(course);
        }

        // Arama İşlemi (SEARCH)
        // GET: /Course/Search?q=yazılım
        public IActionResult Search(string q)
        {
            // Arama boşsa kurslara geri dön
            if (string.IsNullOrEmpty(q)) return RedirectToAction("Index");

            // 1. Veritabanında Ara (Büyük/küçük harf duyarsız)
            var results = _context.Courses
                          .Include(c => c.Category)
                          .Where(x => x.Title.Contains(q) || x.Description.Contains(q))
                          .ToList();

            // 2. Aranan kelimeyi View'e taşı (Madde 7 İsteri)
            // ViewBag: Sadece bu sayfa için
            ViewBag.SearchQuery = q;

            // TempData: Bir sonraki istekte de tutulabilir (Örnek olsun diye kullanıyoruz)
            TempData["InfoMessage"] = $"{results.Count} adet sonuç bulundu.";

            return View(results); // Sonuçları Search.cshtml'e gönderiyoruz
        }

        // GET: Düzenleme Sayfasını Getir
        public IActionResult Edit(int id)
        {
            // Güvenlik: Sadece Admin
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("Index");

            var course = _context.Courses.Find(id);
            if (course == null) return NotFound();

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", course.CategoryId);
            return View(course);
        }

        // POST: Güncellemeyi Kaydet
        [HttpPost]
        public IActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Courses.Update(course);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Kurs başarıyla güncellendi!";
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", course.CategoryId);
            return View(course);
        }

        // --- SİLME (DELETE) ---

        // GET: Silme İşlemi (Direkt siler ve yönlendirir)
        public IActionResult Delete(int id)
        {
            // Güvenlik: Sadece Admin
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("Index");

            var course = _context.Courses.Find(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Kurs silindi!";
            }
            return RedirectToAction("Index");
        }
        // POST: Satın Alma İşlemi
        [HttpPost]
        public IActionResult Buy(int id)
        {
            // 1. Kullanıcı Giriş Yapmış mı?
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["Error"] = "Satın almak için önce giriş yapmalısınız!";
                return RedirectToAction("Login", "Auth");
            }

            // 2. Kursu Bul
            var course = _context.Courses.Find(id);
            if (course == null) return NotFound();

            // 3. Daha önce satın almış mı? (Opsiyonel Kontrol)
            bool alreadyBought = _context.Sales.Any(s => s.UserId == userId && s.CourseId == id);
            if (alreadyBought)
            {
                TempData["InfoMessage"] = "Bu kursu zaten satın aldınız.";
                return RedirectToAction("MyCourses", "Student"); // Birazdan yapacağız
            }

            // 4. Satışı Kaydet
            var sale = new Sale
            {
                UserId = userId.Value,
                CourseId = course.Id,
                Price = course.Price,
                Date = DateTime.Now
            };

            _context.Sales.Add(sale);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Kurs başarıyla satın alındı! İyi dersler.";
            return RedirectToAction("MyCourses", "Student");
        }
    }
}