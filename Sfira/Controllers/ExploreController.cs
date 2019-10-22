using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Controllers
{
    public class ExploreController : Controller
    {
        private readonly IDataStorage dataStorage;
        private readonly UserManager<ApplicationUser> userManager;

        public ExploreController(IDataStorage dataStorage, UserManager<ApplicationUser> userManager)
        {
            this.dataStorage = dataStorage;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<PostViewModel> posts = dataStorage.GetPosts().ToViewModels();

            foreach (var post in posts)
            {
                post.Attachment = dataStorage.GetAttachmentByPostId(post.Id)?.ToViewModel;
            }

            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
                posts = dataStorage.LoadCurrentUserRelations(posts, currentUser.Id);
            }

            return View("Explore", posts);
        }
    }
}
