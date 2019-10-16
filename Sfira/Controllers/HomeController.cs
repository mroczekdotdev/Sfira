using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataStorage dataStorage;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(IDataStorage dataStorage, UserManager<ApplicationUser> userManager)
        {
            this.dataStorage = dataStorage;
            this.userManager = userManager;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<PostViewModel> posts;

            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);

                posts = dataStorage.GetPostsByFollowerId(currentUser.Id).ToViewModels();

                if (posts.Count() > 0)
                {
                    foreach (var post in posts)
                    {
                        post.Attachment = dataStorage.GetAttachmentByPostId(post.Id)?.ToViewModel();
                    }

                    posts = dataStorage.LoadCurrentUserRelations(posts, currentUser.Id);
                }
                else
                {
                    posts = null;
                }
            }
            else
            {
                posts = null;
            }

            return View("Home", posts);
        }
    }
}
