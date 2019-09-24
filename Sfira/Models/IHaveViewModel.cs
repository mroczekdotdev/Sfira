namespace MroczekDotDev.Sfira.Models
{
    public interface IHaveViewModel<T>
    {
        T ToViewModel();
    }
}
