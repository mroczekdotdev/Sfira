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
        private readonly IRepository repository;
        private readonly UserManager<ApplicationUser> userManager;
        private const int PostsFeedCount = 10;

        public ExploreController(IRepository repository, UserManager<ApplicationUser> userManager)
        {
            this.repository = repository;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            IEnumerable<PostViewModel> posts = repository.GetPosts(PostsFeedCount).ToViewModels();
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
            IEnumerable<PostViewModel> posts = repository.GetPosts(count, cursor).ToViewModels();

            return ViewComponent(typeof(PostsFeedViewComponent), posts);
        }
    }
}
