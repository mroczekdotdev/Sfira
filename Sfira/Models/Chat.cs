using MroczekDotDev.Sfira.ViewModels;
using System.Collections.Generic;

namespace MroczekDotDev.Sfira.Models
{
    public abstract class Chat : IHaveViewModel<ChatViewModel>
    {
        public int Id { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<UserChat> UserChats { get; set; }

        public int? LastMessageId { get; set; }
        public Message LastMessage { get; set; }

        public abstract ChatViewModel ToViewModel();
    }
}
