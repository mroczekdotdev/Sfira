namespace MroczekDotDev.Sfira.Models
{
    public class UserFollow
    {
        public string FollowingUserId { get; set; }
        public ApplicationUser FollowingUser { get; set; }

        public string FollowedUserId { get; set; }
        public ApplicationUser FollowedUser { get; set; }
    }
}
