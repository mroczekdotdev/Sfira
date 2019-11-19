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
    public class CommentsFeedViewComponent : ViewComponent
    {
        private readonly IRepository repository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly FeedOptions feedOptions;
        private readonly int commentsFeedCount;

        public CommentsFeedViewComponent(
            IRepository repository,
            UserManager<ApplicationUser> userManager,
            IOptionsMonitor<FeedOptions> feedOptionsAccessor)
        {
            this.repository = repository;
            this.userManager = userManager;
            feedOptions = feedOptionsAccessor.CurrentValue;
            commentsFeedCount = feedOptions.CommentsFeedCount;
        }

        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<CommentViewModel> comments)
        {
            if (comments != null)
            {
                if (comments.Count() < commentsFeedCount)
                {
                    ViewContext.HttpContext.Response.Headers.Add("Loader-Keep", "false");
                }
                else
                {
                    ViewContext.HttpContext.Response.Headers.Add("Loader-Cursor", comments.Last().Id.ToString());
                }
            }

            return View("CommentsFeed", comments);
        }
    }
}
