using MroczekDotDev.Sfira.ViewModels;

namespace MroczekDotDev.Sfira.Models
{
    public class ImageAttachment : Attachment
    {
        public FilenameExtension Extension { get; set; }

        public override AttachmentViewModel ToViewModel()
        {
            return new AttachmentViewModel
            {
                ParentId = PostId,
                Owner = Owner,
                Name = Name.ToString(),
                Type = ToString(),
                Extension = Extension.ToString(),
            };
        }
    }
}
