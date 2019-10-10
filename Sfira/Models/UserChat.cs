using System;

namespace MroczekDotDev.Sfira.Models
{
    public class UserChat
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int ChatId { get; set; }
        public Chat Chat { get; set; }

        public DateTime LastReadTime { get; set; }
    }
}
