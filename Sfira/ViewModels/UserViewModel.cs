using MroczekDotDev.Sfira.Models;
using System;
using System.Collections.Generic;

namespace MroczekDotDev.Sfira.ViewModels
{
    public class UserViewModel : IHasUserMedia
    {
        public string Id { get; set; }
        public DateTime RegisterTime { get; set; }

        public string UserName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Website { get; set; }

        public string AvatarImage { get; set; }
        public string CoverImage { get; set; }

        public PostsFeedLoaderViewModel PostsFeedLoader { get; set; }

        public int FollowingCount { get; set; }
        public int FollowersCount { get; set; }

        public bool IsCurrentUser { get; set; }
        public bool IsFollowedByCurrentUser { get; set; }
        public bool IsBlockedByCurrentUser { get; set; }
    }
}
