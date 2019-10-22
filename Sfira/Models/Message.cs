using MroczekDotDev.Sfira.ViewModels;

namespace MroczekDotDev.Sfira.Models
{
    public class Message : Entry, IHasViewModel<MessageViewModel>
    {
        public int ChatId { get; set; }
        public Chat Chat { get; set; }

        public MessageViewModel ToViewModel => new MessageViewModel
        {
            Id = Id,
            Author = Author,
            PublicationTime = PublicationTime,
            Body = Body,
            ChatId = ChatId,
        };
    }
}
