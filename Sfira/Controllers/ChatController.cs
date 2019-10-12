using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Infrastructure;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Controllers
{
    public class ChatController : Controller
    {
        private readonly IDataStorage dataStorage;
        private readonly UserManager<ApplicationUser> userManager;

        public ChatController(IDataStorage dataStorage, UserManager<ApplicationUser> userManager)
        {
            this.dataStorage = dataStorage;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> DirectChat(string userName)
        {
            ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            ApplicationUser interlocutor = dataStorage.GetUserByUserName(userName);

            ChatViewModel chat = dataStorage.GetDirectChatByUserIds(currentUser.Id, interlocutor.Id)?.ToViewModel();

            if (chat == null)
            {
                chat = new DirectChatViewModel
                {
                    Interlocutor = interlocutor.ToViewModel(),
                };
            }
            else
            {
                chat.Messages = dataStorage.GetMessagesByChatId(chat.Id).ToViewModels();
                chat.Messages = dataStorage.LoadCurrentUserAuthorship(chat.Messages, currentUser.Id);
            }

            return View("Chat", chat);
        }

        [Authorize]
        public async Task<IActionResult> MessagesFeed(int chatId)
        {
            ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);

            UserChat userChat = dataStorage.GetUserChat(currentUser.Id, chatId);

            if (userChat != null)
            {
                IEnumerable<MessageViewModel> messages = dataStorage.GetMessagesByChatId(chatId).ToViewModels();
                messages = dataStorage.LoadCurrentUserAuthorship(messages, currentUser.Id);

                return PartialView("_MessagesFeedPartial", messages);
            }
            else
            {
                return BadRequest();
            }

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] MessageViewModel message, int chatId, string interlocutorId)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);

                if (chatId == 0)
                {
                    chatId = dataStorage.AddDirectChat(currentUser.Id, interlocutorId).Id;
                }

                message.ChatId = chatId;
                message.Author = currentUser;
                Message createdMessage = dataStorage.AddMessage(message);

                if (createdMessage != null)
                {
                    return Ok(chatId);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
