namespace MroczekDotDev.Sfira.Models
{
    public class UserBlock
    {
        public ApplicationUser BlockingUser { get; set; }
        public string BlockingUserId { get; set; }

        public ApplicationUser BlockedUser { get; set; }
        public string BlockedUserId { get; set; }
    }
}
