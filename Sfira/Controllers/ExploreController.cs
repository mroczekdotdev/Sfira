using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewComponents;
using MroczekDotDev.Sfira.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MroczekDotDev.Sfira.Controllers
{
    public class ExploreController : Controller
    {
        private readonly IRepository repository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly FeedOptions feedOptions;
        private readonly int postsFeedCount;

        public static string Name { get; } = nameof(ExploreController)
            .Substring(0, nameof(ExploreController).LastIndexOf(nameof(Controller)));

        public ExploreController(
            IRepository repository,
            UserManager<ApplicationUser> userManager,
            IOptionsMonitor<FeedOptions> feedOptionsAccessor)
        {
            this.repository = repository;
            this.userManager = userManager;
            feedOptions = feedOptionsAccessor.CurrentValue;
            postsFeedCount = feedOptions.PostsFeedCount;
        }

        public IActionResult Index()
        {
            IEnumerable<PostViewModel> posts = repository.GetPosts(postsFeedCount).ToViewModels();
            var postsFeedLoader = new PostsFeedLoaderViewModel();
            postsFeedLoader.Posts = posts;

            if (posts.Count() == postsFeedCount)
            {
                postsFeedLoader.HasLoader = true;
                postsFeedLoader.LoaderLink = "/Explore/PostsFeed/";
                postsFeedLoader.LoaderCount = postsFeedCount;
                postsFeedLoader.LoaderCursor = posts.Last().Id;
            }

            return View("Explore", postsFeedLoader);
        }

        public IActionResult PostsFeed(int count, int cursor)
        {
            IEnumerable<PostViewModel> posts = repository.GetPosts(count, cursor).ToViewModels();

            return ViewComponent(typeof(PostsFeedViewComponent), posts);
        }
    }
}
