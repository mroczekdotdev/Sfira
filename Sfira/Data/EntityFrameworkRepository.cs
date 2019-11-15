using Microsoft.EntityFrameworkCore;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MroczekDotDev.Sfira.Data
{
    public class EntityFrameworkRepository : IRepository
    {
        private readonly PostgreSqlDbContext context;

        public EntityFrameworkRepository(PostgreSqlDbContext context)
        {
            this.context = context;
        }

        public ApplicationUser GetUserById(string userId)
        {
            return context.Users
                .SingleOrDefault(u => u.Id == userId);
        }

        public ApplicationUser GetUserByUserName(string userName)
        {
            return context.Users
                .SingleOrDefault(u => u.UserName == userName);
        }

        public IEnumerable<ApplicationUser> GetFollowersByUserName(string userName)
        {
            return context.UserFollows
                .Where(uf => uf.FollowedUser.UserName == userName)
                .Select(uf => uf.FollowingUser)
                .ToArray();
        }

        public void AddPost(PostViewModel post)
        {
            StringBuilder sb = new StringBuilder();
            string bodyTags = Regex.Replace(post.Body, @"(?:)(#\b(\w+)\b)(?=.*\1)", "", RegexOptions.IgnoreCase);

            foreach (Match match in Regex.Matches(bodyTags, @"#(\w+)"))
            {
                sb.Append(match.Groups[1]).Append(' ');
            }

            var postToAdd = new Post
            {
                Author = post.Author,
                PublicationTime = DateTime.UtcNow,
                Body = post.Body,
                Tags = sb.ToString(),
            };

            context.Posts.Add(postToAdd);

            if (post.Attachment != null)
            {
                AddAttachment(post.Attachment, postToAdd);
            }
            else
            {
                context.SaveChanges();
            }
        }

        public Post GetPostById(int postId)
        {
            return context.Posts
                .Include(p => p.Author)
                .SingleOrDefault(p => p.Id == postId);
        }

        public IEnumerable<Post> GetPosts(int? count = null, int? cursor = null)
        {
            IQueryable<Post> query = context.Posts
                .OrderByDescending(p => p.PublicationTime)
                .ThenByDescending(p => p.Id);

            if (cursor != null)
            {
                DateTime cursorTime = context.Posts
                    .Where(p => p.Id == cursor)
                    .SingleOrDefault()
                    .PublicationTime;

                query = query.Where(
                    p => p.PublicationTime < cursorTime ||
                    (p.PublicationTime == cursorTime && p.Id < cursor));
            }

            if (count != null)
            {
                query = query.Take((int)count);
            }

            return query
                .Include(p => p.Author)
                .ToArray();
        }

        public IEnumerable<Post> GetPostsByTag(string tagName, int? count = null, int? cursor = null)
        {
            IQueryable<Post> query = context.Posts
                .Where(p => p.Tags.Contains(tagName))
                .OrderByDescending(p => p.PublicationTime)
                .ThenByDescending(p => p.Id);

            if (cursor != null)
            {
                DateTime cursorTime = context.Posts
                    .Where(p => p.Id == cursor)
                    .SingleOrDefault()
                    .PublicationTime;

                query = query.Where(
                    p => p.PublicationTime < cursorTime ||
                    (p.PublicationTime == cursorTime && p.Id < cursor));
            }

            if (count != null)
            {
                query = query.Take((int)count);
            }

            return query
                .Include(p => p.Author)
                .ToArray();
        }

        public IEnumerable<Post> GetPostsByUserName(string userName, int? count = null, int? cursor = null)
        {
            IQueryable<Post> query = context.Posts
                .Where(p => p.Author.UserName == userName)
                .OrderByDescending(p => p.PublicationTime)
                .ThenByDescending(p => p.Id);

            if (cursor != null)
            {
                DateTime cursorTime = context.Posts
                    .Where(p => p.Id == cursor)
                    .SingleOrDefault()
                    .PublicationTime;

                query = query.Where(
                    p => p.PublicationTime < cursorTime ||
                    (p.PublicationTime == cursorTime && p.Id < cursor));
            }

            if (count != null)
            {
                query = query.Take((int)count);
            }

            return query
                .Include(p => p.Author)
                .ToArray();
        }

        public IEnumerable<Post> GetPostsByFollowerId(string userId, int? count = null, int? cursor = null)
        {
            IQueryable<Post> query = context.UserFollows
                .Where(uf => uf.FollowingUserId == userId)
                .SelectMany(uf => uf.FollowedUser.Posts)
                .OrderByDescending(p => p.PublicationTime)
                .ThenByDescending(p => p.Id);

            if (cursor != null)
            {
                DateTime cursorTime = context.Posts
                    .Where(p => p.Id == cursor)
                    .SingleOrDefault()
                    .PublicationTime;

                query = query.Where(
                    p => p.PublicationTime < cursorTime ||
                    (p.PublicationTime == cursorTime && p.Id < cursor));
            }

            if (count != null)
            {
                query = query.Take((int)count);
            }

            return query
                .Include(p => p.Author)
                .ToArray();
        }

        public Attachment GetAttachmentByPostId(int postId)
        {
            return context.Attachments
                .Include(a => a.Owner)
                .SingleOrDefault(a => a.ParentId == postId);
        }

        public IEnumerable<Attachment> GetAttachmentsByUserName(string userName)
        {
            return context.Attachments
                .Where(a => a.Owner.UserName == userName)
                .Include(a => a.Owner)
                .ToArray();
        }

        public IEnumerable<PostViewModel> LoadCurrentUserRelations(IEnumerable<PostViewModel> posts, string currentUserId)
        {
            Dictionary<int, RelationType> currentUserRelations = context.Users
                .Where(u => u.Id == currentUserId)
                .SelectMany(up => up.UserPosts)
                .ToDictionary(t => t.PostId, t => t.Relation);

            foreach (PostViewModel p in posts)
            {
                p.CurrentUserRelation = currentUserRelations.TryGetValue(p.Id, out RelationType value) ? value : RelationType.None;
            }

            return posts;
        }

        public UserPost MarkPost(string userId, int postId, string interaction)
        {
            Post post = GetPostById(postId);

            if (userId != post.Author.Id)
            {
                UserPost result;
                UserPost existingUserPost = context.UserPosts.Find(userId, postId);
                RelationType relation = existingUserPost?.Relation ?? RelationType.None;

                switch (interaction)
                {
                    case "like":
                        if ((relation & RelationType.Like) != RelationType.Like)
                        {
                            relation |= RelationType.Like;
                            post.LikesCount++;
                            break;
                        }
                        return null;

                    case "unlike":
                        if ((relation & RelationType.Like) == RelationType.Like)
                        {
                            relation ^= RelationType.Like;
                            post.LikesCount--;
                            break;
                        }
                        return null;

                    case "favorite":
                        if ((relation & RelationType.Favorite) != RelationType.Favorite)
                        {
                            relation |= RelationType.Favorite;
                            post.FavoritesCount++;
                            break;
                        }
                        return null;

                    case "unfavorite":
                        if ((relation & RelationType.Favorite) == RelationType.Favorite)
                        {
                            relation ^= RelationType.Favorite;
                            post.FavoritesCount--;
                            break;
                        }
                        return null;

                    case "comment":
                        if ((relation & RelationType.Comment) != RelationType.Comment)
                        {
                            relation |= RelationType.Comment;
                            break;
                        }
                        return null;

                    case "uncomment":
                        if ((relation & RelationType.Comment) == RelationType.Comment)
                        {
                            relation ^= RelationType.Comment;
                            break;
                        }
                        return null;
                }

                if (existingUserPost == null)
                {
                    var userPost = new UserPost
                    {
                        UserId = userId,
                        PostId = postId,
                        Relation = relation,
                    };

                    context.Add(userPost);
                    result = userPost;
                }
                else if (relation == RelationType.None)
                {
                    context.Remove(existingUserPost);
                    result = existingUserPost;
                }
                else
                {
                    existingUserPost.Relation = relation;
                    result = existingUserPost;
                }

                context.SaveChanges();
                return result;
            }
            else
            {
                return null;
            }
        }

        public void AddComment(CommentViewModel comment)
        {
            Post parent = GetPostById(comment.ParentId);

            var commentToAdd = new Comment
            {
                Author = comment.Author,
                PublicationTime = DateTime.UtcNow,
                Body = comment.Body,
                Parent = parent,
            };

            if (!(parent.Comments?.Any(c => c.Author == comment.Author) ?? false))
            {
                MarkPost(comment.Author.Id, parent.Id, "comment");
            }

            parent.CommentsCount++;
            context.Comments.Add(commentToAdd);
            context.SaveChanges();
        }

        public IEnumerable<Comment> GetCommentsByPostId(int postId)
        {
            return context.Comments
                .Where(c => c.Parent.Id == postId)
                .OrderByDescending(c => c.PublicationTime)
                .Include(c => c.Author)
                .Include(c => c.Parent)
                .ToArray();
        }

        public DirectChat AddDirectChat(string userId, string interlocutorId)
        {
            if (GetDirectChatByUserIds(userId, interlocutorId) == null)
            {
                var chatToAdd = new DirectChat();

                var userChatForUser = new UserChat
                {
                    UserId = userId,
                    Chat = chatToAdd,
                };

                var userChatForInterlocutor = new UserChat
                {
                    UserId = interlocutorId,
                    Chat = chatToAdd,
                };

                context.AddRange(chatToAdd, userChatForUser, userChatForInterlocutor);
                context.SaveChanges();

                return chatToAdd;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<Chat> GetChatsListByUserId(string userId)
        {
            IEnumerable<Chat> chats = context.UserChats
                .Where(uc => uc.UserId == userId)
                .Select(uc => uc.Chat)
                .OrderByDescending(c => c.LastMessage.PublicationTime)
                .Include(c => c.UserChats)
                    .ThenInclude(uc => uc.User)
                .Include(c => c.LastMessage)
                   .ThenInclude(m => m.Author)
                .ToArray();

            if (chats.Any())
            {
                UserChat userChat = GetUserChat(userId, chats.First().Id);

                foreach (Chat chat in chats)
                {
                    chat.UserChats.Remove(userChat);
                }
            }

            return chats;
        }

        public Chat GetChatById(int chatId)
        {
            return context.Chats
                .Where(c => c.Id == chatId)
                .Include(c => c.UserChats)
                    .ThenInclude(uc => uc.User)
                .Include(c => c.LastMessage)
                    .ThenInclude(m => m.Author)
                .SingleOrDefault();
        }

        public DirectChat GetDirectChatByUserIds(string userId, string interlocutorId)
        {
            DirectChat directChat = context.Chats
                .OfType<DirectChat>()
                .Where(c => c.UserChats.Any(uc => uc.UserId == userId))
                .Where(c => c.UserChats.Any(uc => uc.UserId == interlocutorId))
                .Include(c => c.UserChats)
                    .ThenInclude(uc => uc.User)
                .SingleOrDefault();

            if (directChat != null)
            {
                UserChat userChat = GetUserChat(userId, directChat.Id);
                directChat.UserChats.Remove(userChat);
            }

            return directChat;
        }

        public UserChat GetUserChat(string userId, int chatId)
        {
            return context.UserChats.Find(userId, chatId);
        }

        public Message AddMessage(MessageViewModel message)
        {
            UserChat existingUserChat = context.UserChats
                .Where(uc => uc.UserId == message.Author.Id)
                .Where(uc => uc.ChatId == message.ChatId)
                .SingleOrDefault();

            if (existingUserChat != null)
            {
                var messageToAdd = new Message
                {
                    Author = message.Author,
                    PublicationTime = DateTime.UtcNow,
                    Body = message.Body,
                    ChatId = message.ChatId,
                };

                context.Chats.Find(message.ChatId).LastMessage = messageToAdd;
                context.Messages.Add(messageToAdd);
                context.SaveChanges();

                return messageToAdd;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<Message> GetMessagesByChatId(int chatId)
        {
            return context.Messages
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.PublicationTime)
                .Include(m => m.Author)
                .ToArray();
        }

        public IEnumerable<MessageViewModel> LoadCurrentUserAuthorship(
            IEnumerable<MessageViewModel> messages, string currentUserId)
        {
            foreach (MessageViewModel message in messages)
            {
                message.IsCurrentUserAuthor = message.Author.Id == currentUserId;
            }

            return messages;
        }

        public UserFollow GetUserFollow(string followingUserId, string followedUserId)
        {
            return context.UserFollows.Find(followingUserId, followedUserId);
        }

        public UserFollow AddUserFollow(string followingUserId, string followedUserUsername)
        {
            ApplicationUser followingUser;
            ApplicationUser followedUser;
            UserFollow existingUserFollow;

            if ((followedUser = GetUserByUserName(followedUserUsername)) != null
                && followingUserId != followedUser.Id
                && (followingUser = GetUserById(followingUserId)) != null
                && (existingUserFollow = GetUserFollow(followingUserId, followedUser.Id)) == null)
            {
                var userFollow = new UserFollow
                {
                    FollowingUserId = followingUser.Id,
                    FollowedUserId = followedUser.Id,
                };

                followingUser.FollowingCount++;
                followedUser.FollowersCount++;
                context.Add(userFollow);
                context.SaveChanges();
                return userFollow;
            }
            else
            {
                return null;
            }
        }

        public UserFollow RemoveUserFollow(string followingUserId, string followedUserUsername)
        {
            ApplicationUser followingUser;
            ApplicationUser followedUser;
            UserFollow userFollow;

            if ((followedUser = GetUserByUserName(followedUserUsername)) != null
                && followingUserId != followedUser.Id
                && (followingUser = GetUserById(followingUserId)) != null
                && (userFollow = GetUserFollow(followingUserId, followedUser.Id)) != null)
            {
                followingUser.FollowingCount--;
                followedUser.FollowersCount--;
                context.Remove(userFollow);
                context.SaveChanges();
                return userFollow;
            }
            else
            {
                return null;
            }
        }

        private void AddAttachment(AttachmentViewModel attachment, Post parent)
        {
            Attachment attachmentToAdd;

            switch (Enum.Parse<FileType>(attachment.Type))
            {
                case FileType.image:
                    attachmentToAdd = new ImageAttachment
                    {
                        Parent = parent,
                        Owner = attachment.Owner,
                        Name = Guid.ParseExact(attachment.Name, "D"),
                        Extension = Enum.Parse<FilenameExtension>(attachment.Extension),
                    };
                    break;

                default:
                    return;
            }

            context.Attachments.Add(attachmentToAdd);
            context.SaveChanges();
        }
    }
}
