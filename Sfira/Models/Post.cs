using System;
using System.Collections.Generic;

namespace MarcinMroczek.Sfira.Models
{
    public class Post
    {
        public int Id { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime PostTime { get; set; }

        public string Message { get; set; }
        public string Tags { get; set; }

        public int LikesCount { get; set; }
        public int FavoritesCount { get; set; }
        public ICollection<UserPost> UserPosts { get; set; }
    }
}
