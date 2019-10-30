using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Services.CachedStorage;
using System.Linq;

namespace MroczekDotDev.Sfira.ViewComponents

{
    public class TrendingTagsViewComponent : ViewComponent
    {
        private const int trendingTagsMaxCount = 6;
        private readonly TrendingTagsCached cache;

        public TrendingTagsViewComponent(TrendingTagsCached cache)
        {
            this.cache = cache;
        }

        public IViewComponentResult Invoke()
        {
            return View("TrendingTags", cache.items.Take(trendingTagsMaxCount));
        }
    }
}
