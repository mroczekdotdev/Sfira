using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Controllers
{
    public class ChatController : Controller
    {
        private readonly IRepository repository;
        private readonly UserManager<ApplicationUser> userManager;

        public static string Name { get; } = nameof(ChatController)
            .Substring(0, nameof(ChatController).LastIndexOf(nameof(Controller)));

        public ChatController(IRepository repository, UserManager<ApplicationUser> userManager)
        {
            this.repository = repository;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> DirectChat(string userName)
        {
            ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);

            if (!userName.Equals(currentUser.UserName, StringComparison.OrdinalIgnoreCase))
            {
                ApplicationUser interlocutor = repository.GetUserByUserName(userName);
                ChatViewModel chat = repository.GetDirectChatByUserIds(currentUser.Id, interlocutor.Id)?.ToViewModel;

                if (chat == null)
                {
                    chat = new DirectChatViewModel { Interlocutor = interlocutor.ToViewModel };
                }
                else
                {
                    chat.Messages = repository.GetMessagesByChatId(chat.Id).ToViewModels();
                    chat.Messages = repository.LoadCurrentUserAuthorship(chat.Messages, currentUser.Id);
                }

                return View("Chat", chat);
            }
            else
            {
                return RedirectToAction(nameof(MessagesController.Index), "Messages");
            }
        }

        [Authorize]
        public async Task<IActionResult> MessagesFeed(int chatId)
        {
            ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);

            UserChat userChat = repository.GetUserChat(currentUser.Id, chatId);

            if (userChat != null)
            {
                IEnumerable<MessageViewModel> messages = repository.GetMessagesByChatId(chatId).ToViewModels();
                messages = repository.LoadCurrentUserAuthorship(messages, currentUser.Id);

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
                    chatId = repository.AddDirectChat(currentUser.Id, interlocutorId).Id;
                }

                message.ChatId = chatId;
                message.Author = currentUser;
                Message createdMessage = repository.AddMessage(message);

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
