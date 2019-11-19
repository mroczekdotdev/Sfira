using System.Collections.Generic;

namespace MroczekDotDev.Sfira.ViewModels
{
    public class PostsFeedLoaderViewModel : FeedLoaderViewModel
    {
        public IEnumerable<PostViewModel> Posts { get; set; }
    }
}
