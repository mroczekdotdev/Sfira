using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Services.Scheduling
{
    public class TrendingUsersScheduledTask : IScheduledTask
    {
        private ILogger<TrendingUsersScheduledTask> logger;

        public TrendingUsersScheduledTask(ILogger<TrendingUsersScheduledTask> logger)
        {
            this.logger = logger;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            logger.Log(LogLevel.Critical, "TrendingUsersScheduledTask: NotImplemented");
        }
    }
}
