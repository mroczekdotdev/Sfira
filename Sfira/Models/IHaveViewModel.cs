using System.Collections.Generic;
using System.Linq;

namespace MroczekDotDev.Sfira.Models
{
    public interface IHaveViewModel<T>
    {
        T ToViewModel();
    }

    public static class Extensions
    {
        public static IEnumerable<T> ToViewModels<T>(this IEnumerable<IHaveViewModel<T>> models)
        {
            return models.Select(m => m.ToViewModel()).ToArray();
        }
    }
}
