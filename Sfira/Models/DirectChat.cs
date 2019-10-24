using MroczekDotDev.Sfira.ViewModels;
using System.Linq;

namespace MroczekDotDev.Sfira.Models
{
    public class DirectChat : Chat
    {
        public override ChatViewModel ToViewModel => new DirectChatViewModel
        {
            Id = Id,
            Messages = Messages.ToViewModels(),
            Interlocutor = UserChats.Last().User?.ToViewModel,
            LastMessage = LastMessage?.ToViewModel,
        };
    }
}
