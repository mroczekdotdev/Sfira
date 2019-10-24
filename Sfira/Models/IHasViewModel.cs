using System.Collections.Generic;
using System.Linq;

namespace MroczekDotDev.Sfira.Models
{
    public interface IHasViewModel<T>
    {
        T ToViewModel { get; }
    }

    public static class Extensions
    {
        public static IEnumerable<T> ToViewModels<T>(this IEnumerable<IHasViewModel<T>> models)
        {
            return models?.Select(m => m.ToViewModel).ToArray();
        }
    }
}
