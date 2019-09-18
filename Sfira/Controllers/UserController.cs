using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            UserViewModel result = dataStorage.GetUserVmByUserName(userName);
            result.Posts = dataStorage.GetPostsVmByUserName(userName);

            foreach (var post in result.Posts)
            {
                post.Attachment = dataStorage.GetAttachmentVmByPostId(post.Id);
            }

            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
                result.Posts = dataStorage.AddCurrentUserRelations(result.Posts, currentUser.Id);
            }

            return View("User", result);
        }
    }
}
