using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using System.Collections.Generic;

namespace MroczekDotDev.Sfira.Data
{
    public interface IDataStorage
    {
        ApplicationUser GetUserById(string userId);
        ApplicationUser GetUserByUserName(string userName);

        Post GetPostById(int postId);
        IEnumerable<Post> GetPosts();
        IEnumerable<Post> GetPostsByTag(string tag);
        IEnumerable<Post> GetPostsByUserName(string userName);

        void AddPost(PostViewModel post);
        Attachment GetAttachmentByPostId(int postId);

        void MarkPost(string userId, int postId, string interaction);
        IEnumerable<PostViewModel> LoadCurrentUserRelations(IEnumerable<PostViewModel> posts, string currentUserId);

        void AddComment(CommentViewModel post);
        IEnumerable<Comment> GetCommentsByPostId(int postId);

        UserFollow GetUserFollow(string followingUserId, string followedUserId);
        UserFollow AddUserFollow(string followingUserId, string followedUserUsername);
        UserFollow RemoveUserFollow(string followingUserId, string followedUserUsername);
    }
}
