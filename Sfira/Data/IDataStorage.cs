using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using System.Collections.Generic;

namespace MroczekDotDev.Sfira.Data
{
    public interface IDataStorage
    {
        ApplicationUser GetUserById(string userId);
        ApplicationUser GetUserByUserName(string userName);
        IEnumerable<ApplicationUser> GetFollowersByUserName(string userName);

        void AddPost(PostViewModel post);
        Post GetPostById(int postId);
        IEnumerable<Post> GetPosts();
        IEnumerable<Post> GetPostsByTag(string tagName);
        IEnumerable<Post> GetPostsByUserName(string userName);
        IEnumerable<Post> GetPostsByFollowerId(string userId);

        Attachment GetAttachmentByPostId(int postId);
        IEnumerable<Attachment> GetAttachmentsByUserName(string userName);

        UserPost MarkPost(string userId, int postId, string interaction);
        IEnumerable<PostViewModel> LoadCurrentUserRelations(IEnumerable<PostViewModel> posts, string currentUserId);

        void AddComment(CommentViewModel comment);
        IEnumerable<Comment> GetCommentsByPostId(int postId);

        DirectChat AddDirectChat(string userId, string interlocutorId);
        IEnumerable<Chat> GetChatsListByUserId(string userId);
        Chat GetChatById(int chatId);
        DirectChat GetDirectChatByUserIds(string userId, string interlocutorId);

        Message AddMessage(MessageViewModel message);
        IEnumerable<Message> GetMessagesByChatId(int chatId);
        IEnumerable<MessageViewModel> LoadCurrentUserAuthorship(IEnumerable<MessageViewModel> messages, string currentUserId);

        UserFollow GetUserFollow(string followingUserId, string followedUserId);
        UserFollow AddUserFollow(string followingUserId, string followedUserUsername);
        UserFollow RemoveUserFollow(string followingUserId, string followedUserUsername);
    }
}
