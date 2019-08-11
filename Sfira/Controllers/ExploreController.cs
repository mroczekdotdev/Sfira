using MarcinMroczek.Sfira.Data;
using MarcinMroczek.Sfira.Models;
using MarcinMroczek.Sfira.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarcinMroczek.Sfira.Controllers
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
            IEnumerable<PostViewModel> result = dataStorage.GetAllPosts();

            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
                result = dataStorage.AddCurrentUserRelations(result, currentUser.Id);
            }

            return View("Explore", result);
        }
    }
}
