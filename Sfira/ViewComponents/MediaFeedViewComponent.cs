using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.ViewComponents
{
    public class MediaFeedViewComponent : ViewComponent
    {
        private readonly IRepository repository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly FeedOptions feedOptions;
        private readonly int mediaFeedCount;

        public MediaFeedViewComponent(
            IRepository repository,
            UserManager<ApplicationUser> userManager,
            IOptionsMonitor<FeedOptions> feedOptionsAccessor)
        {
            this.repository = repository;
            this.userManager = userManager;
            feedOptions = feedOptionsAccessor.CurrentValue;
            mediaFeedCount = feedOptions.MediaFeedCount;
        }

        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<AttachmentViewModel> media)
        {
            if (media != null)
            {
                if (media.Count() < mediaFeedCount)
                {
                    ViewContext.HttpContext.Response.Headers.Add("Loader-Keep", "false");
                }
                else
                {
                    ViewContext.HttpContext.Response.Headers.Add("Loader-Cursor", media.Last().ParentId.ToString());
                }
            }

            return View("MediaFeed", media);
        }
    }
}
