using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewComponents;
using MroczekDotDev.Sfira.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository repository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly FeedOptions feedOptions;
        private readonly int postsFeedCount;

        public HomeController(
            IRepository repository,
            UserManager<ApplicationUser> userManager,
            IOptionsMonitor<FeedOptions> feedOptionsAccessor)
        {
            this.repository = repository;
            this.userManager = userManager;
            feedOptions = feedOptionsAccessor.CurrentValue;
            postsFeedCount = feedOptions.PostsFeedCount;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            PostsFeedLoaderViewModel postsFeedLoader = null;

            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
                IEnumerable<PostViewModel> posts = repository.GetPostsByFollowerId(currentUser.Id, postsFeedCount).ToViewModels();

                if (posts.Any())
                {
                    postsFeedLoader = new PostsFeedLoaderViewModel();
                    postsFeedLoader.Posts = posts;

                    if (posts.Count() == postsFeedCount)
                    {
                        postsFeedLoader.HasLoader = true;
                        postsFeedLoader.LoaderLink = "/PostsFeed/";
                        postsFeedLoader.LoaderCount = postsFeedCount;
                        postsFeedLoader.LoaderCursor = posts.Last().Id;
                    }
                }
            }

            return View("Home", postsFeedLoader);
        }

        [Authorize]
        public async Task<IActionResult> PostsFeed(int count, int cursor)
        {
            ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);

            IEnumerable<PostViewModel> posts = repository.GetPostsByFollowerId(currentUser.Id, count, cursor).ToViewModels();

            return ViewComponent(typeof(PostsFeedViewComponent), posts);
        }
    }
}
