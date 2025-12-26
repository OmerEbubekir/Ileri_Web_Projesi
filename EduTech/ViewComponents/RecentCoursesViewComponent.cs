using EduTech.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduTech.ViewComponents
{
    public class RecentCoursesViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public RecentCoursesViewComponent(AppDbContext context)
        {
            _context = context;
        }

        // Bu metod ViewComponent çağrıldığında çalışır
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Son eklenen 3 kursu getir (Id'ye göre tersten sırala)
            var recentCourses = await _context.Courses
                                      .OrderByDescending(x => x.Id)
                                      .Take(3)
                                      .ToListAsync();

            return View(recentCourses);
        }
    }
}