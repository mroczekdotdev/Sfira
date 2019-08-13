using System;

namespace MarcinMroczek.Sfira.Models
{
    public abstract class Entry
    {
        public int Id { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime PublicationTime { get; set; }
        public string Message { get; set; }
    }
}
