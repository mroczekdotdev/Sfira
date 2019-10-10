using MroczekDotDev.Sfira.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace MroczekDotDev.Sfira.ViewModels
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime PublicationTime { get; set; }

        [Required]
        [StringLength(240, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Body { get; set; }

        [Required]
        public int ChatId { get; set; }

        public bool IsCurrentUserAuthor { get; set; }
    }
}
