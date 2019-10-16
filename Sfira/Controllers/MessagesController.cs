using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IDataStorage dataStorage;
        private readonly UserManager<ApplicationUser> userManager;

        public MessagesController(IDataStorage dataStorage, UserManager<ApplicationUser> userManager)
        {
            this.dataStorage = dataStorage;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            IEnumerable<ChatViewModel> chats = dataStorage.GetChatsListByUserId(currentUser.Id).ToViewModels();

            foreach (ChatViewModel chat in chats)
            {
                chat.LastMessage.IsCurrentUserAuthor = chat.LastMessage.Author == currentUser ? true : false;
            }

            return View("Messages", chats);
        }
    }
}
