﻿using MarcinMroczek.Sfira.Models;
using MarcinMroczek.Sfira.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MarcinMroczek.Sfira.Data
{
    public interface IDataStorage
    {
        void AddPost(PostViewModel post);
        PostViewModel GetPostById(int postId);
        void MarkPost(string userId, int postId, string interaction);
        IEnumerable<PostViewModel> AddCurrentUserRelations(IEnumerable<PostViewModel> posts, string currentUserId);

        IEnumerable<PostViewModel> GetAllPosts();
        IEnumerable<PostViewModel> GetPostsByTag(string tag);
        IEnumerable<PostViewModel> GetPostsByUserName(string userName);

        UserViewModel GetUserByUserName(string userName);

    }
}
