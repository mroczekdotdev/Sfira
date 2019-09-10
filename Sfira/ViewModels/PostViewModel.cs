using MarcinMroczek.Sfira.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarcinMroczek.Sfira.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime PublicationTime { get; set; }

        [Required]
        [StringLength(240, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Message { get; set; }

        public string Tags { get; set; }

        public int LikesCount { get; set; }
        public int FavoritesCount { get; set; }
        public RelationType CurrentUserRelation { get; set; }

        public int CommentsCount { get; set; }
        public ICollection<CommentViewModel> Comments { get; set; }

        public AttachmentViewModel Attachment { get; set; }
    }
}
