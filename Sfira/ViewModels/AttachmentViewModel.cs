using MroczekDotDev.Sfira.Models;

namespace MroczekDotDev.Sfira.ViewModels
{
    public class AttachmentViewModel
    {
        public int ParentId { get; set; }
        public ApplicationUser Owner { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
    }
}
