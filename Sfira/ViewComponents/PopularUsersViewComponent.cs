using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Services.CachedStorage;
using System.Linq;
using MroczekDotDev.Sfira.Models;
using Microsoft.Extensions.Options;
using MroczekDotDev.Sfira.Services.Scheduling;

namespace MroczekDotDev.Sfira.ViewComponents

{
    public class PopularUsersViewComponent : ViewComponent
    {
        private readonly PopularUsersCached cache;
        private readonly PopularContentOptions options;
        private readonly int popularUsersCount;

        public PopularUsersViewComponent(
            PopularUsersCached cache, IOptionsMonitor<PopularContentOptions> optionsAccessor)
        {
            this.cache = cache;
            options = optionsAccessor.CurrentValue;
            popularUsersCount = options.PopularUsersCount;
        }

        public IViewComponentResult Invoke()
        {
            while (cache.Items.IsDefault)
            {
                //wait for service
            }

            return View("PopularUsers", cache.Items.Take(popularUsersCount).ToViewModels());
        }
    }
}
