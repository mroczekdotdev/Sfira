using System.Collections.Generic;

namespace MroczekDotDev.Sfira.ViewModels
{
    public class PostsFeedLoaderViewModel
    {
        public IEnumerable<PostViewModel> Posts { get; set; }

        public bool HasLoader { get; set; }
        public string LoaderLink { get; set; }
        public int LoaderCount { get; set; }
        public int LoaderCursor { get; set; }
    }
}
