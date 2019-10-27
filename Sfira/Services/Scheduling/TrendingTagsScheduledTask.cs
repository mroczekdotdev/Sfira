using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Services.Scheduling
{
    public class TrendingTagsScheduledTask : IScheduledTask
    {
        private ILogger<TrendingTagsScheduledTask> logger;

        public TrendingTagsScheduledTask(ILogger<TrendingTagsScheduledTask> logger)
        {
            this.logger = logger;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            logger.Log(LogLevel.Critical, "TrendingTagsScheduledTask: NotImplemented");
        }
    }
}
