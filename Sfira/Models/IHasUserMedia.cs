namespace MroczekDotDev.Sfira.Models
{
    public interface IHasUserMedia
    {
        string Id { get; set; }
        string AvatarImage { get; set; }
        string CoverImage { get; set; }
    }
}
