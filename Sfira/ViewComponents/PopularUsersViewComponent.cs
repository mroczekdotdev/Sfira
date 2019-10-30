using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Services.CachedStorage;
using System.Linq;
using MroczekDotDev.Sfira.Models;

namespace MroczekDotDev.Sfira.ViewComponents

{
    public class PopularUsersViewComponent : ViewComponent
    {
        private const int popularUsersMaxCount = 6;
        private readonly PopularUsersCached cache;

        public PopularUsersViewComponent(PopularUsersCached cache)
        {
            this.cache = cache;
        }

        public IViewComponentResult Invoke()
        {
            return View("PopularUsers", cache.items.Take(popularUsersMaxCount).ToViewModels());
        }
    }
}
