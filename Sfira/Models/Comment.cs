using MroczekDotDev.Sfira.ViewModels;

namespace MroczekDotDev.Sfira.Models
{
    public class Comment : Entry, IHaveViewModel<CommentViewModel>
    {
        public Post Parent { get; set; }

        public CommentViewModel ToViewModel()
        {
            return new CommentViewModel
            {
                Id = Id,
                Author = Author,
                PublicationTime = PublicationTime,
                Message = Message,
                ParentId = Parent.Id,
            };
        }
    }
}
