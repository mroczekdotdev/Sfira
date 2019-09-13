using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MroczekDotDev.Sfira.Data
{
    public interface IDataStorage
    {
        void AddPost(PostViewModel post);
        PostViewModel GetPostVmById(int postId);
        void MarkPost(string userId, int postId, string interaction);
        IEnumerable<PostViewModel> AddCurrentUserRelations(IEnumerable<PostViewModel> posts, string currentUserId);

        IEnumerable<PostViewModel> GetPostsVm();
        IEnumerable<PostViewModel> GetPostsVmByTag(string tag);
        IEnumerable<PostViewModel> GetPostsVmByUserName(string userName);

        AttachmentViewModel GetAttachmentVmByPostId(int postId);

        UserViewModel GetUserVmByUserName(string userName);
        IEnumerable<CommentViewModel> GetCommentsVmByPostId(int postId);
        void AddComment(CommentViewModel post);
    }
}
