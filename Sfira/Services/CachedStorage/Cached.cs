using System.Collections.Immutable;

namespace MroczekDotDev.Sfira.Services.CachedStorage
{
    public abstract class Cached<T>
    {
        protected const int maxCount = 10;

        public abstract ImmutableArray<T> items { get; set; }

        public abstract void Reload();
    }
}
