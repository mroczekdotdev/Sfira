using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.ViewComponents
{
    public class PostsFeedViewComponent : ViewComponent
    {
        private readonly IRepository repository;
        private readonly UserManager<ApplicationUser> userManager;
        private const int PostsFeedCount = 10;

        public PostsFeedViewComponent(IRepository repository, UserManager<ApplicationUser> userManager)
        {
            this.repository = repository;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<PostViewModel> posts)
        {
            foreach (var post in posts)
            {
                post.Attachment = repository.GetAttachmentByPostId(post.Id)?.ToViewModel;
            }

            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
                posts = repository.LoadCurrentUserRelations(posts, currentUser.Id);
            }

            if (posts.Count() < PostsFeedCount)
            {
                ViewContext.HttpContext.Response.Headers.Add("Loader-Keep", "false");
            }
            else
            {
                ViewContext.HttpContext.Response.Headers.Add("Loader-Cursor", posts.Last().Id.ToString());
            }

            return View("PostsFeed", posts);
        }
    }
}
