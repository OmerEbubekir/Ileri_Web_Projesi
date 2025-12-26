namespace EduTech.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Gerçek hayatta şifreli tutulur, proje için plain text olabilir.
        public string Role { get; set; } // "Admin" veya "Student" olacak
    }
}