using Microsoft.Extensions.Options;
using MroczekDotDev.Sfira.Services.CachedStorage;
using System.Threading;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Services.Scheduling
{
    public class PopularUsersTask : IScheduledTask
    {
        private readonly PopularUsersCached cache;
        private readonly PopularContentOptions options;

        public PopularUsersTask(PopularUsersCached cache, IOptionsMonitor<PopularContentOptions> optionsAccessor)
        {
            this.cache = cache;
            options = optionsAccessor.CurrentValue;
        }

        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cache.Reload(options.PeriodInMinutes, options.SamplesPerMinute);

            return Task.CompletedTask;
        }
    }
}
