using MroczekDotDev.Sfira.Services.CachedStorage;
using System.Threading;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Services.Scheduling
{
    public class TrendingTagsScheduledTask : IScheduledTask
    {
        private readonly TrendingTagsCached cache;

        public TrendingTagsScheduledTask(TrendingTagsCached cache)
        {
            this.cache = cache;
        }

        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cache.Reload();
            return Task.CompletedTask;
        }
    }
}
