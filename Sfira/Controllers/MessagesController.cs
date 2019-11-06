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
        private readonly IRepository repository;
        private readonly UserManager<ApplicationUser> userManager;

        public static string Name { get; } = nameof(MessagesController)
            .Substring(0, nameof(MessagesController).LastIndexOf(nameof(Controller)));

        public MessagesController(IRepository repository, UserManager<ApplicationUser> userManager)
        {
            this.repository = repository;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            IEnumerable<ChatViewModel> chats = repository.GetChatsListByUserId(currentUser.Id).ToViewModels();

            foreach (ChatViewModel chat in chats)
            {
                chat.LastMessage.IsCurrentUserAuthor = chat.LastMessage.Author == currentUser ? true : false;
            }

            return View("Messages", chats);
        }
    }
}
