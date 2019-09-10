using MarcinMroczek.Sfira.Models;
using MarcinMroczek.Sfira.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MarcinMroczek.Sfira.Data
{
    public interface IDataStorage
    {
        void AddPost(PostViewModel post);
        PostViewModel GetPostVmById(int postId);
        void MarkPost(string userId, int postId, string interaction);
        IEnumerable<PostViewModel> AddCurrentUserRelations(IEnumerable<PostViewModel> posts, string currentUserId);

        IEnumerable<PostViewModel> GetAllPosts();
        IEnumerable<PostViewModel> GetPostsByTag(string tag);
        IEnumerable<PostViewModel> GetPostsByUserName(string userName);

        AttachmentViewModel GetAttachmentVmByPostId(int postId);

        UserViewModel GetUserByUserName(string userName);
        IEnumerable<CommentViewModel> GetCommentsByPostId(int postId);
        void AddComment(CommentViewModel post);
    }
}
