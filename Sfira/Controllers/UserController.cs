using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewComponents;
using MroczekDotDev.Sfira.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Controllers
{
    public class UserController : Controller
    {
        private readonly IRepository repository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly FeedOptions feedOptions;
        private readonly int postsFeedCount;
        private readonly int mediaFeedCount;

        public static string Name { get; } = nameof(UserController)
            .Substring(0, nameof(UserController).LastIndexOf(nameof(Controller)));

        public UserController(
            IRepository repository,
            UserManager<ApplicationUser> userManager,
            IOptionsMonitor<FeedOptions> feedOptionsAccessor)
        {
            this.repository = repository;
            this.userManager = userManager;
            feedOptions = feedOptionsAccessor.CurrentValue;
            postsFeedCount = feedOptions.PostsFeedCount;
            mediaFeedCount = feedOptions.MediaFeedCount;
        }

        public async Task<IActionResult> Index(string userName)
        {
            UserViewModel user = repository.GetUserByUserName(userName)?.ToViewModel;

            if (user != null)
            {
                ApplicationUser currentUser = null;

                if (User.Identity.IsAuthenticated)
                {
                    currentUser = await userManager.FindByNameAsync(User.Identity.Name);

                    if (currentUser.Id == user.Id)
                    {
                        user.IsCurrentUser = true;
                    }
                    else
                    {
                        user.IsFollowedByCurrentUser = repository.GetUserFollow(currentUser.Id, user.Id) != null;
                    }
                }

                var posts = repository.GetPostsAndFavoritesByUserName(userName, postsFeedCount).ToViewModels();

                if (posts.Any())
                {
                    posts = AddFavoritedBy(posts, user.Name);

                    var postsFeedLoader = new PostsFeedLoaderViewModel
                    {
                        Posts = posts
                    };

                    if (posts.Count() == postsFeedCount)
                    {
                        postsFeedLoader.HasLoader = true;
                        postsFeedLoader.LoaderLink = user.UserName + "/PostsFeed/";
                        postsFeedLoader.LoaderCount = postsFeedCount;
                        postsFeedLoader.LoaderCursor = posts.Last().Id;
                    }
                    user.PostsFeedLoader = postsFeedLoader;
                }
            }

            return View("User", user);
        }

        public IActionResult PostsFeed(string userName, int count, int cursor)
        {
            ApplicationUser user = repository.GetUserByUserName(userName);
            IEnumerable<PostViewModel> posts = null;

            if (user != null)
            {
                posts = repository.GetPostsAndFavoritesByUserName(userName, count, cursor).ToViewModels();

                if (posts.Any())
                {
                    posts = AddFavoritedBy(posts, user.Name);
                }
            }

            return ViewComponent(typeof(PostsFeedViewComponent), posts);
        }

        public IActionResult Media(string userName)
        {
            IEnumerable<AttachmentViewModel> media =
                repository.GetAttachmentsByUserName(userName, mediaFeedCount).ToViewModels();

            var mediaFeedLoader = new MediaFeedLoaderViewModel
            {
                Media = media
            };

            if (media.Any() && media.Count() == mediaFeedCount)
            {
                mediaFeedLoader.HasLoader = true;
                mediaFeedLoader.LoaderLink = userName + "/MediaFeed/";
                mediaFeedLoader.LoaderCount = mediaFeedCount;
                mediaFeedLoader.LoaderCursor = media.Last().ParentId;
            }

            return PartialView("_MediaFeedLoaderPartial", mediaFeedLoader);
        }

        public IActionResult MediaFeed(string userName, int count, int cursor)
        {
            IEnumerable<AttachmentViewModel> media =
                repository.GetAttachmentsByUserName(userName, count, cursor).ToViewModels();
            return ViewComponent(typeof(MediaFeedViewComponent), media);
        }

        public IActionResult Followers(string userName)
        {
            IEnumerable<UserViewModel> followers = repository.GetFollowersByUserName(userName).ToViewModels();
            return PartialView("_FollowersFeedPartial", followers);
        }

        [Authorize]
        public async Task<IActionResult> Follow(string userName)
        {
            ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            UserFollow userFollow = repository.AddUserFollow(currentUser.Id, userName);

            if (userFollow != null)
            {
                string followersCount = repository.GetUserByUserName(userName).FollowersCount.ToString();
                return Ok(followersCount);
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize]
        public async Task<IActionResult> Unfollow(string userName)
        {
            ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            UserFollow userFollow = repository.RemoveUserFollow(currentUser.Id, userName);

            if (userFollow != null)
            {
                string followersCount = repository.GetUserByUserName(userName).FollowersCount.ToString();
                return Ok(followersCount);
            }
            else
            {
                return BadRequest();
            }
        }

        [NonAction]
        private IEnumerable<PostViewModel> AddFavoritedBy(IEnumerable<PostViewModel> posts, string name)
        {
            foreach (var post in posts)
            {
                if (!post.Author.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    post.FavoritedBy = name;
                }
            }

            return posts;
        }
    }
}
