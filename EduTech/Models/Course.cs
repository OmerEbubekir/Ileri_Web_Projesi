namespace EduTech.Models
{
    public class Course
    {
        public int Id { get; set; }

        public string Title { get; set; } // Bu zorunlu kalsın

        public string? Description { get; set; } // Soru işareti (?) koyduk: Artık boş geçilebilir (Nullable)

        public string? ImageUrl { get; set; } // Soru işareti (?) koyduk: Resim girmezse hata vermesin

        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; } // Soru işareti koyduk: Validation sırasında ilişkiyi kontrol etmeye çalışıp hata vermesin
    }
}