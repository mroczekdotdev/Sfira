using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MroczekDotDev.Sfira.Services.CachedStorage;
using MroczekDotDev.Sfira.Services.Scheduling;
using System.Linq;

namespace MroczekDotDev.Sfira.ViewComponents

{
    public class TrendingTagsViewComponent : ViewComponent
    {
        private readonly TrendingTagsCached cache;
        private readonly PopularContentOptions options;
        private readonly int trendingTagsCount;

        public TrendingTagsViewComponent(
            TrendingTagsCached cache, IOptionsMonitor<PopularContentOptions> optionsAccessor)
        {
            this.cache = cache;
            options = optionsAccessor.CurrentValue;
            trendingTagsCount = options.TrendingTagsCount;
        }

        public IViewComponentResult Invoke()
        {
            while (cache.Items.IsDefault)
            {
                //wait for service
            }

            return View("TrendingTags", cache.Items.Take(trendingTagsCount));
        }
    }
}
