namespace MroczekDotDev.Sfira.ViewModels
{
    public abstract class FeedLoaderViewModel
    {
        public bool HasLoader { get; set; }
        public string LoaderLink { get; set; }
        public int LoaderCount { get; set; }
        public int LoaderCursor { get; set; }
    }
}
