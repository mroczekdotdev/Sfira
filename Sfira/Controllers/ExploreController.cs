using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<ViewResult> Index()
        {
            IEnumerable<PostViewModel> result = dataStorage.GetPostsVm();

            foreach (var post in result)
            {
                post.Attachment = dataStorage.GetAttachmentVmByPostId(post.Id);
            }

            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
                result = dataStorage.AddCurrentUserRelations(result, currentUser.Id);
            }

            return View("Explore", result);
        }
    }
}
