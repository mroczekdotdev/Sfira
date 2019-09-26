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
    public class EfDataStorage : IDataStorage
    {
        private readonly SfiraDbContext context;

        public EfDataStorage(SfiraDbContext context)
        {
            this.context = context;
        }

        public ApplicationUser GetUserById(string userId)
        {
            var TEMP = context.Users
                .SingleOrDefault(u => u.Id == userId);
            return TEMP;
        }

        public ApplicationUser GetUserByUserName(string userName)
        {
            return context.Users
                .SingleOrDefault(u => u.UserName == userName);
        }

        public Post GetPostById(int postId)
        {
            return context.Posts
                .Where(p => p.Id == postId)
                .SingleOrDefault();
        }

        public IEnumerable<Post> GetPosts()
        {
            return context.Posts
                .OrderByDescending(p => p.PublicationTime)
                .Include(p => p.Author)
                .ToArray();
        }

        public IEnumerable<Post> GetPostsByTag(string tag)
        {
            return context.Posts
                .Where(p => p.Tags.Contains(tag))
                .OrderByDescending(p => p.PublicationTime)
                .Include(p => p.Author)
                .ToArray();
        }

        public IEnumerable<Post> GetPostsByUserName(string userName)
        {
            return context.Posts
                .Where(p => p.Author.UserName == userName)
                .OrderByDescending(p => p.PublicationTime)
                .Include(p => p.Author)
                .ToArray();
        }

        public void AddPost(PostViewModel post)
        {
            StringBuilder sb = new StringBuilder();
            string messageTags = Regex.Replace(post.Message, @"(?:)#\b(\w+)\b(?=.*\1)", "");

            foreach (Match match in Regex.Matches(messageTags, @"#(\w+)"))
            {
                sb.Append(match.Groups[1]).Append(' ');
            }

            var postToAdd = new Post
            {
                Author = post.Author,
                PublicationTime = DateTime.Now,
                Message = post.Message,
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

        public Attachment GetAttachmentByPostId(int postId)
        {
            return context.Attachments
                .Include(a => a.Owner)
                .SingleOrDefault(a => a.PostId == postId);
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

        public void MarkPost(string userId, int postId, string interaction)
        {
            Post post = context.Posts.Find(postId);
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
                    return;

                case "unlike":
                    if ((relation & RelationType.Like) == RelationType.Like)
                    {
                        relation ^= RelationType.Like;
                        post.LikesCount--;
                        break;
                    }
                    return;

                case "favorite":
                    if ((relation & RelationType.Favorite) != RelationType.Favorite)
                    {
                        relation |= RelationType.Favorite;
                        post.FavoritesCount++;
                        break;
                    }
                    return;

                case "unfavorite":
                    if ((relation & RelationType.Favorite) == RelationType.Favorite)
                    {
                        relation ^= RelationType.Favorite;
                        post.FavoritesCount--;
                        break;
                    }
                    return;

                case "comment":
                    if ((relation & RelationType.Comment) != RelationType.Comment)
                    {
                        relation |= RelationType.Comment;
                        break;
                    }
                    return;

                case "uncomment":
                    if ((relation & RelationType.Comment) == RelationType.Comment)
                    {
                        relation ^= RelationType.Comment;
                        break;
                    }
                    return;
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
            }
            else if (relation == RelationType.None)
            {
                context.Remove(existingUserPost);
            }
            else
            {
                existingUserPost.Relation = relation;
            }

            context.SaveChanges();
        }

        public void AddComment(CommentViewModel comment)
        {
            Post parent = GetPostById(comment.ParentId);

            var commentToAdd = new Comment
            {
                Author = comment.Author,
                PublicationTime = DateTime.Now,
                Message = comment.Message,
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

        public UserFollow GetUserFollow(string followingUserId, string followedUserId)
        {
            return context.UserFollow.Find(followingUserId, followedUserId);
        }

        public UserFollow AddUserFollow(string followingUserId, string followedUserUsername)
        {
            ApplicationUser followingUser;
            ApplicationUser followedUser;
            UserFollow existingUserFollow;

            if ((followedUser = GetUserByUserName(followedUserUsername)) != null
                && followingUserId != followedUser.Id
                && (followingUser = GetUserById(followingUserId)) != null
                && (existingUserFollow = context.UserFollow.Find(followingUserId, followedUser.Id)) == null)
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
                && (userFollow = context.UserFollow.Find(followingUserId, followedUser.Id)) != null)
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

            switch (Enum.Parse<AttachmentType>(attachment.Type))
            {
                case AttachmentType.image:
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
