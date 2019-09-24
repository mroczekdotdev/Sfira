using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Controllers
{
    public class PostController : Controller
    {
        private readonly IDataStorage dataStorage;
        private readonly UserManager<ApplicationUser> userManager;

        public PostController(IDataStorage dataStorage, UserManager<ApplicationUser> userManager)
        {
            this.dataStorage = dataStorage;
            this.userManager = userManager;
        }

        //[Authorize]
        //[HttpPost]
        //public async Task<RedirectResult> Create(PostViewModel post, string returnUrl = null)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        post.Author = await userManager.FindByNameAsync(User.Identity.Name);
        //        dataStorage.AddPost(post);
        //    }

        //    return Redirect(returnUrl);
        //}

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostViewModel post)
        {
            ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);

            if (ModelState.IsValid)
            {
                post.Author = currentUser;

                if (post.Attachment != null)
                {
                    post.Attachment.Owner = currentUser;
                }

                dataStorage.AddPost(post);
            }
            else
            {
                return Json("error");
            }

            return Json("success");
        }

        [Authorize]
        public async Task<JsonResult> Mark(int postId, string interaction)
        {
            ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            dataStorage.MarkPost(currentUser.Id, postId, interaction);
            PostViewModel post = dataStorage.GetPostById(postId).ToViewModel();
            var result = new
            {
                likescount = post.LikesCount,
                favoritescount = post.FavoritesCount,
            };

            return Json(result);
        }
    }
}
