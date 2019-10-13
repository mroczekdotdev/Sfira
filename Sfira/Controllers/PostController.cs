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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostViewModel post)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
                post.Author = currentUser;

                if (post.Attachment != null)
                {
                    post.Attachment.Owner = currentUser;
                }

                dataStorage.AddPost(post);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize]
        public async Task<IActionResult> Mark(int postId, string interaction)
        {
            ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            UserPost userPost = dataStorage.MarkPost(currentUser.Id, postId, interaction);

            if (userPost != null)
            {
                PostViewModel post = dataStorage.GetPostById(postId).ToViewModel();

                var postCounters = new
                {
                    likescount = post.LikesCount,
                    favoritescount = post.FavoritesCount,
                };

                return Ok(postCounters);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
