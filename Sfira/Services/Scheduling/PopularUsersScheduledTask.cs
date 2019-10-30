using MroczekDotDev.Sfira.Services.CachedStorage;
using System.Threading;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Services.Scheduling
{
    public class PopularUsersScheduledTask : IScheduledTask
    {
        private readonly PopularUsersCached cache;

        public PopularUsersScheduledTask(PopularUsersCached cache)
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
