namespace EduTech.Models
{
    public class Sale
    {
        public int Id { get; set; }

        // Kim aldı?
        public int UserId { get; set; }
        public User User { get; set; } // İlişki

        // Neyi aldı?
        public int CourseId { get; set; }
        public Course Course { get; set; } // İlişki

        // Ne zaman aldı?
        public DateTime Date { get; set; }

        // Kaça aldı?
        public decimal Price { get; set; }
    }
}