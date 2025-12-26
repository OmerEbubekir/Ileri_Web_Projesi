namespace EduTech.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } // Örn: Yazılım, Tasarım

        // Bir kategoride birden fazla kurs olabilir
        public List<Course>? Courses { get; set; }
    }
}