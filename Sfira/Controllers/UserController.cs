﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Infrastructure;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
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

        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            UserViewModel result = dataStorage.GetUserByUserName(userName).ToViewModel();
            result.Posts = dataStorage.GetPostsByUserName(userName).ToViewModels();

            foreach (var post in result.Posts)
            {
                post.Attachment = dataStorage.GetAttachmentByPostId(post.Id)?.ToViewModel();
            }

            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
                result.Posts = dataStorage.LoadCurrentUserRelations(result.Posts, currentUser.Id);

                if (currentUser.Id == result.Id)
                {
                    result.IsCurrentUser = true;
                }
                else
                {
                    result.IsFollowedByCurrentUser = dataStorage.GetUserFollow(currentUser.Id, result.Id) != null;
                }
            }

            return View("User", result);
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
