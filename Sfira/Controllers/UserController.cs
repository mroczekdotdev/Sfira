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
    public class UserController : Controller
    {
        private readonly IDataStorage dataStorage;
        private readonly UserManager<ApplicationUser> userManager;
        private const int PostsFeedCount = 10;

        public UserController(IDataStorage dataStorage, UserManager<ApplicationUser> userManager)
        {
            this.dataStorage = dataStorage;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(string userName)
        {
            UserViewModel user = dataStorage.GetUserByUserName(userName)?.ToViewModel;

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
                        user.IsFollowedByCurrentUser = dataStorage.GetUserFollow(currentUser.Id, user.Id) != null;
                    }
                }

                var posts = dataStorage.GetPostsByUserName(userName, PostsFeedCount).ToViewModels();

                if (posts.Any())
                {
                    var postsFeedLoader = new PostsFeedLoaderViewModel();
                    postsFeedLoader.Posts = posts;

                    if (posts.Count() == PostsFeedCount)
                    {
                        postsFeedLoader.HasLoader = true;
                        postsFeedLoader.LoaderLink = user.UserName + "/PostsFeed/";
                        postsFeedLoader.LoaderCount = PostsFeedCount;
                        postsFeedLoader.LoaderCursor = posts.Last().Id;
                    }
                    user.PostsFeedLoader = postsFeedLoader;
                }
            }

            return View("User", user);
        }

        public IActionResult PostsFeed(string userName, int count, int cursor)
        {
            IEnumerable<PostViewModel> posts = dataStorage.GetPostsByUserName(userName, count, cursor).ToViewModels();

            return ViewComponent(typeof(PostsFeedViewComponent), posts);
        }

        public PartialViewResult Followers(string userName)
        {
            IEnumerable<UserViewModel> followers = dataStorage.GetFollowersByUserName(userName).ToViewModels();
            return PartialView("_FollowersFeedPartial", followers);
        }

        public PartialViewResult Media(string userName)
        {
            IEnumerable<AttachmentViewModel> attachments = dataStorage.GetAttachmentsByUserName(userName).ToViewModels();
            return PartialView("_MediaFeedPartial", attachments);
        }

        [Authorize]
        public async Task<IActionResult> Follow(string userName)
        {
            ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            UserFollow userFollow = dataStorage.AddUserFollow(currentUser.Id, userName);

            if (userFollow != null)
            {
                string followersCount = dataStorage.GetUserByUserName(userName).FollowersCount.ToString();
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
            UserFollow userFollow = dataStorage.RemoveUserFollow(currentUser.Id, userName);

            if (userFollow != null)
            {
                string followersCount = dataStorage.GetUserByUserName(userName).FollowersCount.ToString();
                return Ok(followersCount);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
