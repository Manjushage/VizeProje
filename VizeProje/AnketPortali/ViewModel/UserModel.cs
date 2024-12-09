namespace AnketPortali.ViewModel
{
    public class UserModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // "Free" veya "Admin"
        public DateTime CreatedAt { get; set; }
    }
}
