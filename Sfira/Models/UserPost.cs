﻿using System;

namespace MroczekDotDev.Sfira.Models
{
    [Flags]
    public enum RelationType
    {
        None = 0,
        Like = 1,
        Favorite = 2,
        Comment = 4,
    }

    public class UserPost
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public RelationType Relation { get; set; }
    }
}
