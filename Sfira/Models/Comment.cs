using MroczekDotDev.Sfira.ViewModels;

namespace MroczekDotDev.Sfira.Models
{
    public class Comment : Entry, IHasViewModel<CommentViewModel>
    {
        public int ParentId { get; set; }
        public Post Parent { get; set; }

        public CommentViewModel ToViewModel => new CommentViewModel
        {
            Id = Id,
            Author = Author,
            PublicationTime = PublicationTime,
            Body = Body,
            ParentId = Parent.Id,
        };
    }
}
