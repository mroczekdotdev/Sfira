using MroczekDotDev.Sfira.ViewModels;

namespace MroczekDotDev.Sfira.Models
{
    public class ImageAttachment : Attachment
    {
        public FilenameExtension Extension { get; set; }

        public override AttachmentViewModel ToViewModel => new AttachmentViewModel
        {
            ParentId = ParentId,
            Owner = Owner,
            Name = Name.ToString(),
            Type = ToString(),
            Extension = Extension.ToString(),
        };
    }
}
