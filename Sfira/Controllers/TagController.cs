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
    public class TagController : Controller
    {
        private readonly IRepository repository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly FeedOptions feedOptions;
        private readonly int postsFeedCount;

        public TagController(
            IRepository repository,
            UserManager<ApplicationUser> userManager,
            IOptionsMonitor<FeedOptions> feedOptionsAccessor)
        {
            this.repository = repository;
            this.userManager = userManager;
            feedOptions = feedOptionsAccessor.CurrentValue;
            postsFeedCount = feedOptions.PostsFeedCount;
        }

        public IActionResult Index(string tagName)
        {
            PostsFeedLoaderViewModel postsFeedLoader = null;
            IEnumerable<PostViewModel> posts = repository.GetPostsByTag(tagName, postsFeedCount).ToViewModels();

            if (posts.Any())
            {
                postsFeedLoader = new PostsFeedLoaderViewModel();
                postsFeedLoader.Posts = posts;

                if (posts.Count() == postsFeedCount)
                {
                    postsFeedLoader.HasLoader = true;
                    postsFeedLoader.LoaderLink = "/Tag/" + tagName + "/PostsFeed/";
                    postsFeedLoader.LoaderCount = postsFeedCount;
                    postsFeedLoader.LoaderCursor = posts.Last().Id;
                }
            }

            return View("Tag", postsFeedLoader);
        }

        public IActionResult PostsFeed(string tagName, int count, int cursor)
        {
            IEnumerable<PostViewModel> posts = repository.GetPostsByTag(tagName, count, cursor).ToViewModels();

            return ViewComponent(typeof(PostsFeedViewComponent), posts);
        }
    }
}
