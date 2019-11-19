using System.Collections.Generic;

namespace MroczekDotDev.Sfira.ViewModels
{
    public class MediaFeedLoaderViewModel : FeedLoaderViewModel
    {
        public IEnumerable<AttachmentViewModel> Media { get; set; }
    }
}
