namespace MroczekDotDev.Sfira.Models
{
    public class UserFollow
    {
        public ApplicationUser FollowingUser { get; set; }
        public string FollowingUserId { get; set; }

        public ApplicationUser FollowedUser { get; set; }
        public string FollowedUserId { get; set; }
    }
}
