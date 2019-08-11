using System;
using System.Collections.Generic;

namespace MarcinMroczek.Sfira.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public DateTime RegisterTime { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Website { get; set; }
        public bool ProfileImage { get; set; }
        public bool HeaderImage { get; set; }

        public IEnumerable<PostViewModel> Posts { get; set; }
    }
}
