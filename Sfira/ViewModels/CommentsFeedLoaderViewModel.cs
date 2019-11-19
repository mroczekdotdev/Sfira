using System.Collections.Generic;

namespace MroczekDotDev.Sfira.ViewModels
{
    public class CommentsFeedLoaderViewModel : FeedLoaderViewModel
    {
        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}
