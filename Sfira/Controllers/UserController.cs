using MarcinMroczek.Sfira.Data;
using MarcinMroczek.Sfira.Models;
using MarcinMroczek.Sfira.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarcinMroczek.Sfira.Controllers
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
            UserViewModel result = dataStorage.GetUserByUserName(userName);
            result.Posts = dataStorage.GetPostsByUserName(userName);

            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
                result.Posts = dataStorage.AddCurrentUserRelations(result.Posts, currentUser.Id);
            }

            return View("User", result);
        }
    }
}
