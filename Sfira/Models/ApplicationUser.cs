using Microsoft.AspNetCore.Identity;
using MroczekDotDev.Sfira.ViewModels;
using System;
using System.Collections.Generic;

namespace MroczekDotDev.Sfira.Models
{
    public class ApplicationUser : IdentityUser, IHaveViewModel<UserViewModel>
    {
        public DateTime RegisterTime { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Website { get; set; }

        public bool ProfileImage { get; set; }
        public bool HeaderImage { get; set; }

        public string CountryRegion { get; set; }
        public string Language { get; set; }
        public string TimeZone { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<UserPost> UserPosts { get; set; }

        public ICollection<Attachment> Attachments { get; set; }

        public ICollection<UserFollow> Following { get; set; }
        public int FollowingCount { get; set; }

        public ICollection<UserFollow> Followers { get; set; }
        public int FollowersCount { get; set; }

        public ICollection<UserBlock> Blocking { get; set; }

        public UserViewModel ToViewModel()
        {
            return new UserViewModel
            {
                Id = Id,
                RegisterTime = RegisterTime,
                UserName = UserName,
                Name = Name,
                Description = Description,
                Location = Location,
                Website = Website,
                ProfileImage = ProfileImage,
                HeaderImage = HeaderImage,
                FollowingCount = FollowingCount,
                FollowersCount = FollowersCount,
            };
        }
    }
}
