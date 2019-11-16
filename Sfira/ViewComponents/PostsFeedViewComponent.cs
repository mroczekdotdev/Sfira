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
    public class PostsFeedViewComponent : ViewComponent
    {
        private readonly IRepository repository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly FeedOptions feedOptions;
        private readonly int postsFeedCount;

        public PostsFeedViewComponent(
            IRepository repository,
            UserManager<ApplicationUser> userManager,
            IOptionsMonitor<FeedOptions> feedOptionsAccessor)
        {
            this.repository = repository;
            this.userManager = userManager;
            feedOptions = feedOptionsAccessor.CurrentValue;
            postsFeedCount = feedOptions.PostsFeedCount;
        }

        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<PostViewModel> posts)
        {
            if (posts != null)
            {
                foreach (var post in posts)
                {
                    post.Attachment = repository.GetAttachmentByPostId(post.Id)?.ToViewModel;
                }

                if (User.Identity.IsAuthenticated)
                {
                    ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
                    posts = repository.LoadCurrentUserRelations(posts, currentUser.Id);
                }

                if (posts.Count() < postsFeedCount)
                {
                    ViewContext.HttpContext.Response.Headers.Add("Loader-Keep", "false");
                }
                else
                {
                    ViewContext.HttpContext.Response.Headers.Add("Loader-Cursor", posts.Last().Id.ToString());
                }
            }
            else
            {
                posts = Enumerable.Empty<PostViewModel>();
            }

            return View("PostsFeed", posts);
        }
    }
}
