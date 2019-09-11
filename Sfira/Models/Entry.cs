using System;

namespace MroczekDotDev.Sfira.Models
{
    public abstract class Entry
    {
        public int Id { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime PublicationTime { get; set; }
        public string Message { get; set; }
    }
}
