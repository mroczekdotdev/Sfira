using MroczekDotDev.Sfira.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace MroczekDotDev.Sfira.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime PublicationTime { get; set; }

        [Required]
        [StringLength(240, ErrorMessage = "{0} length must be between {2} and {1} characters.", MinimumLength = 3)]
        [Display(Name = "Comment")]
        public string Body { get; set; }

        [Required]
        public int ParentId { get; set; }
    }
}
