namespace MarcinMroczek.Sfira.Models
{
    public class Comment : Entry
    {
        public Post Parent { get; set; }
    }
}
