using Microsoft.Extensions.Options;
using System.Collections.Immutable;

namespace MroczekDotDev.Sfira.Services.CachedStorage
{
    public abstract class Cached<T>
    {
        protected readonly CachedOptions options;
        protected int maxCount;

        public abstract ImmutableArray<T> Items { get; set; }

        protected Cached(IOptionsMonitor<CachedOptions> optionsAccessor)
        {
            options = optionsAccessor.CurrentValue;
            maxCount = options.MaxCount;
        }

        public abstract void Reload(int periodInMinutes, int samplesPerMinute);
    }
}
