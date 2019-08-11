using MarcinMroczek.Sfira.Models;
using MarcinMroczek.Sfira.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MarcinMroczek.Sfira.Data
{
    public class EFDataStorage : IDataStorage
    {
        private readonly SfiraDbContext context;

        public EFDataStorage(SfiraDbContext context)
        {
            this.context = context;
        }

        public UserViewModel GetUserByUserName(string userName)
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
                    HeaderImage = s.HeaderImage
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

            var postToAdd = new Post
            {
                Author = post.Author,
                PostTime = DateTime.Now,
                Message = post.Message,
                Tags = sb.ToString()
            };

            context.Posts.Add(postToAdd);
            context.SaveChanges();
        }

        public IEnumerable<PostViewModel> GetAllPosts()
        {
            return context.Posts
                .OrderBy(p => p.PostTime)
                .Select(s => new PostViewModel
                {
                    Id = s.Id,
                    Author = s.Author,
                    PostTime = s.PostTime,
                    Message = s.Message,
                    LikesCount = s.LikesCount,
                    FavoritesCount = s.FavoritesCount
                }).ToArray();
        }

        public IEnumerable<PostViewModel> GetPostsByTag(string tag)
        {
            return context.Posts
                .Where(p => p.Tags.Contains(tag))
                .OrderBy(p => p.PostTime)
                .Select(s => new PostViewModel
                {
                    Id = s.Id,
                    Author = s.Author,
                    PostTime = s.PostTime,
                    Message = s.Message,
                    LikesCount = s.LikesCount,
                    FavoritesCount = s.FavoritesCount
                }).ToArray();
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

        public IEnumerable<PostViewModel> GetPostsByUserName(string userName)
        {
            return context.Users
                .Where(u => u.UserName == userName)
                .SelectMany(p => p.Posts)
                .OrderBy(p => p.PostTime)
                .Select(s => new PostViewModel
                {
                    Id = s.Id,
                    Author = s.Author,
                    PostTime = s.PostTime,
                    Message = s.Message,
                    LikesCount = s.LikesCount,
                    FavoritesCount = s.FavoritesCount
                }).ToArray();
        }

        public PostViewModel GetPostById(int postId)
        {
            return context.Posts
                .Where(p => p.Id == postId)
                .Select(s => new PostViewModel
                {
                    Id = s.Id,
                    Author = s.Author,
                    PostTime = s.PostTime,
                    Message = s.Message,
                    LikesCount = s.LikesCount,
                    FavoritesCount = s.FavoritesCount
                }).SingleOrDefault();
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
            }

            if (existingUserPost == null)
            {
                UserPost userPost = new UserPost
                {
                    UserId = userId,
                    PostId = postId,
                    Relation = relation
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
    }
}
