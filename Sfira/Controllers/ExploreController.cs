using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IDataStorage dataStorage;
        private readonly UserManager<ApplicationUser> userManager;
        private const int PostsFeedCount = 10;

        public ExploreController(IDataStorage dataStorage, UserManager<ApplicationUser> userManager)
        {
            this.dataStorage = dataStorage;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            IEnumerable<PostViewModel> posts = dataStorage.GetPosts(PostsFeedCount).ToViewModels();
            var postsFeedLoader = new PostsFeedLoaderViewModel();
            postsFeedLoader.Posts = posts;

            if (posts.Count() == PostsFeedCount)
            {
                postsFeedLoader.HasLoader = true;
                postsFeedLoader.LoaderLink = "/Explore/PostsFeed/";
                postsFeedLoader.LoaderCount = PostsFeedCount;
                postsFeedLoader.LoaderCursor = posts.Last().Id;
            }

            return View("Explore", postsFeedLoader);
        }

        public IActionResult PostsFeed(int count, int cursor)
        {
            IEnumerable<PostViewModel> posts = dataStorage.GetPosts(count, cursor).ToViewModels();

            return ViewComponent(typeof(PostsFeedViewComponent), posts);
        }
    }
}
