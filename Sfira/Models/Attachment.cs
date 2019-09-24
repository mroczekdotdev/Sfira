using MroczekDotDev.Sfira.ViewModels;
using System;

namespace MroczekDotDev.Sfira.Models
{
    public enum AttachmentType
    {
        image = 1,
    }

    public enum FilenameExtension
    {
        jpg = 1,
    }

    public abstract class Attachment : IHaveViewModel<AttachmentViewModel>
    {
        public int PostId { get; set; }
        public Post Parent { get; set; }
        public ApplicationUser Owner { get; set; }
        public Guid Name { get; set; }

        public override string ToString()
        {
            string typeName = GetType().Name.ToLower();
            return typeName.Remove(typeName.Length - 10);
        }

        public abstract AttachmentViewModel ToViewModel();
    }
}
