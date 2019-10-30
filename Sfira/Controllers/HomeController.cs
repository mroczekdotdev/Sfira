using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private const int PostsFeedCount = 10;

        public HomeController(IRepository repository, UserManager<ApplicationUser> userManager)
        {
            this.repository = repository;
            this.userManager = userManager;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            PostsFeedLoaderViewModel postsFeedLoader = null;

            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
                IEnumerable<PostViewModel> posts = repository.GetPostsByFollowerId(currentUser.Id, PostsFeedCount).ToViewModels();

                if (posts.Any())
                {
                    postsFeedLoader = new PostsFeedLoaderViewModel();
                    postsFeedLoader.Posts = posts;

                    if (posts.Count() == PostsFeedCount)
                    {
                        postsFeedLoader.HasLoader = true;
                        postsFeedLoader.LoaderLink = "/PostsFeed/";
                        postsFeedLoader.LoaderCount = PostsFeedCount;
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
