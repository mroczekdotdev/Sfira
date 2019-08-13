using MarcinMroczek.Sfira.Data;
using MarcinMroczek.Sfira.Models;
using MarcinMroczek.Sfira.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarcinMroczek.Sfira.Controllers
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

        [Authorize]
        [HttpPost]
        public async Task<RedirectResult> Create(PostViewModel post, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                post.Author = await userManager.FindByNameAsync(User.Identity.Name);
                dataStorage.AddPost(post);
            }

            return Redirect(returnUrl);
        }

        [Authorize]
        public async Task<JsonResult> Mark(int postId, string interaction)
        {
            ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            dataStorage.MarkPost(currentUser.Id, postId, interaction);
            PostViewModel post = dataStorage.GetPostById(postId);
            var result = new
            {
                likescount = post.LikesCount,
                favoritescount = post.FavoritesCount,
            };

            return Json(result);
        }
    }
}