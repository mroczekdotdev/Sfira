using MroczekDotDev.Sfira.ViewModels;
using System.Collections.Generic;

namespace MroczekDotDev.Sfira.Models
{
    public class Post : Entry, IHasViewModel<PostViewModel>
    {
        public string Tags { get; set; }

        public ICollection<UserPost> UserPosts { get; set; }
        public int LikesCount { get; set; }
        public int FavoritesCount { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public int CommentsCount { get; set; }

        public Attachment Attachment { get; set; }

        public PostViewModel ToViewModel => new PostViewModel
        {
            Id = Id,
            Author = Author,
            PublicationTime = PublicationTime,
            Body = Body,
            LikesCount = LikesCount,
            FavoritesCount = FavoritesCount,
            CommentsCount = CommentsCount,
        };
    }
}
