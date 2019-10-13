using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Infrastructure;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Controllers
{
    public class UserController : Controller
    {
        private readonly IDataStorage dataStorage;
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(IDataStorage dataStorage, UserManager<ApplicationUser> userManager)
        {
            this.dataStorage = dataStorage;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(string userName)
        {
            UserViewModel user = dataStorage.GetUserByUserName(userName)?.ToViewModel();

            if (user != null)
            {
                user.Posts = dataStorage.GetPostsByUserName(userName)?.ToViewModels();

                foreach (var post in user.Posts)
                {
                    post.Attachment = dataStorage.GetAttachmentByPostId(post.Id)?.ToViewModel();
                }

                if (User.Identity.IsAuthenticated)
                {
                    ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
                    user.Posts = dataStorage.LoadCurrentUserRelations(user.Posts, currentUser.Id);

                    if (currentUser.Id == user.Id)
                    {
                        user.IsCurrentUser = true;
                    }
                    else
                    {
                        user.IsFollowedByCurrentUser = dataStorage.GetUserFollow(currentUser.Id, user.Id) != null;
                    }
                }
            }

            return View("User", user);
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
    }
}
