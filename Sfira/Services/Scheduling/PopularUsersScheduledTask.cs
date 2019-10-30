using Microsoft.Extensions.Logging;
using MroczekDotDev.Sfira.Services.CachedStorage;
using System.Threading;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Services.Scheduling
{
    public class PopularUsersScheduledTask : IScheduledTask
    {
        private readonly PopularUsersCached cache;
        private readonly ILogger<PopularUsersScheduledTask> logger;

        public PopularUsersScheduledTask(PopularUsersCached cache, ILogger<PopularUsersScheduledTask> logger)
        {
            this.cache = cache;
            this.logger = logger;
        }

        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cache.Reload();
            return Task.CompletedTask;
        }
    }
}
