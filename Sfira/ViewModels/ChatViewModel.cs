using System.Collections.Generic;

namespace MroczekDotDev.Sfira.ViewModels
{
    public class ChatViewModel
    {
        public int Id { get; set; }
        public IEnumerable<MessageViewModel> Messages { get; set; }
        public MessageViewModel LastMessage { get; set; }
    }
}
