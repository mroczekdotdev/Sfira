﻿using MroczekDotDev.Sfira.Models;
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

        public UserViewModel GetUserVmByUserName(string userName)
        {
            return context.Users
                .Where(u => u.UserName == userName)
                .Select(s => new UserViewModel
                {
                    Id = s.Id,
                    RegisterTime = s.RegisterTime,
                    UserName = s.UserName,
                    Name = s.Name,
                    Description = s.Description,
                    Location = s.Location,
                    Website = s.Website,
                    ProfileImage = s.ProfileImage,
                    HeaderImage = s.HeaderImage,
                }).SingleOrDefault();
        }

        public void AddPost(PostViewModel post)
        {
            StringBuilder sb = new StringBuilder();
            string messageTags = Regex.Replace(post.Message, @"(?:)#\b(\w+)\b(?=.*\1)", "");

            foreach (Match match in Regex.Matches(messageTags, @"#(\w+)"))
            {
                sb.Append(match.Groups[1]).Append(' ');
            }

            Post postToAdd = new Post
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

        public void AddAttachment(AttachmentViewModel attachment, Post parent)
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

        public AttachmentViewModel GetAttachmentVmByPostId(int postId)
        {
            Attachment attachment = context.Attachments
                .SingleOrDefault(a => a.PostId == postId);

            AttachmentViewModel result;

            if (attachment == null)
            {
                result = null;
            }
            else
            {
                result = new AttachmentViewModel
                {
                    ParentId = attachment.PostId,
                    Owner = attachment.Owner,
                    Name = attachment.Name.ToString(),
                };

                switch (attachment)
                {
                    case ImageAttachment image:
                        result.Type = attachment.ToString();
                        result.Extension = image.Extension.ToString();
                        break;

                    default:
                        break;
                }
            }

            return result;
        }

        public IEnumerable<PostViewModel> GetPostsVm()
        {
            return context.Posts
                .OrderByDescending(p => p.PublicationTime)
                .Select(s => new PostViewModel
                {
                    Id = s.Id,
                    Author = s.Author,
                    PublicationTime = s.PublicationTime,
                    Message = s.Message,
                    LikesCount = s.LikesCount,
                    FavoritesCount = s.FavoritesCount,
                    CommentsCount = s.CommentsCount,
                }).ToArray();
        }

        public IEnumerable<PostViewModel> GetPostsVmByTag(string tag)
        {
            return context.Posts
                .Where(p => p.Tags.Contains(tag))
                .OrderByDescending(p => p.PublicationTime)
                .Select(s => new PostViewModel
                {
                    Id = s.Id,
                    Author = s.Author,
                    PublicationTime = s.PublicationTime,
                    Message = s.Message,
                    LikesCount = s.LikesCount,
                    FavoritesCount = s.FavoritesCount,
                    CommentsCount = s.CommentsCount,
                }).ToArray();
        }

        public IEnumerable<PostViewModel> GetPostsVmByUserName(string userName)
        {
            return context.Users
                .Where(u => u.UserName == userName)
                .SelectMany(p => p.Posts)
                .OrderByDescending(p => p.PublicationTime)
                .Select(s => new PostViewModel
                {
                    Id = s.Id,
                    Author = s.Author,
                    PublicationTime = s.PublicationTime,
                    Message = s.Message,
                    LikesCount = s.LikesCount,
                    FavoritesCount = s.FavoritesCount,
                    CommentsCount = s.CommentsCount,
                }).ToArray();
        }

        public PostViewModel GetPostVmById(int postId)
        {
            return context.Posts
                .Where(p => p.Id == postId)
                .Select(s => new PostViewModel
                {
                    Id = s.Id,
                    Author = s.Author,
                    PublicationTime = s.PublicationTime,
                    Message = s.Message,
                    LikesCount = s.LikesCount,
                    FavoritesCount = s.FavoritesCount,
                    CommentsCount = s.CommentsCount,
                }).SingleOrDefault();
        }

        public Post GetPostById(int postId)
        {
            return context.Posts
                .SingleOrDefault(p => p.Id == postId);
        }

        public IEnumerable<PostViewModel> AddCurrentUserRelations(IEnumerable<PostViewModel> posts, string currentUserId)
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
                UserPost userPost = new UserPost
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

            Comment commentToAdd = new Comment
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

        public IEnumerable<CommentViewModel> GetCommentsVmByPostId(int postId)
        {
            return context.Posts
                .Where(p => p.Id == postId)
                .SelectMany(c => c.Comments)
                .OrderByDescending(c => c.PublicationTime)
                .Select(s => new CommentViewModel
                {
                    Id = s.Id,
                    Author = s.Author,
                    PublicationTime = s.PublicationTime,
                    Message = s.Message,
                    ParentId = s.Parent.Id,
                }).ToArray();
        }
    }
}