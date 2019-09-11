using System.Collections.Generic;

namespace MroczekDotDev.Sfira.Models
{
    public class Post : Entry
    {
        public string Tags { get; set; }

        public int LikesCount { get; set; }
        public int FavoritesCount { get; set; }
        public ICollection<UserPost> UserPosts { get; set; }

        public int CommentsCount { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public Attachment Attachment { get; set; }
    }
}
