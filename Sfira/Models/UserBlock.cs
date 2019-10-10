namespace MroczekDotDev.Sfira.Models
{
    public class UserBlock
    {
        public string BlockingUserId { get; set; }
        public ApplicationUser BlockingUser { get; set; }

        public string BlockedUserId { get; set; }
        public ApplicationUser BlockedUser { get; set; }
    }
}
