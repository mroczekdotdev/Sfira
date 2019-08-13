using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace MarcinMroczek.Sfira.Models
{
    public class ApplicationUser : IdentityUser
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
    }
}
