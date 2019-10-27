using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Services.Scheduling
{
    public class PeriodicScheduledJob : IScheduledJob
    {
        private readonly IEnumerable<IScheduledTask> tasks;
        private readonly TimeSpan interval;
        private DateTime LastRun { get; set; }
        private DateTime NextRun { get; set; }

        public PeriodicScheduledJob(IEnumerable<IScheduledTask> tasks, DateTime currentTime, TimeSpan interval)
        {
            NextRun = currentTime;
            this.interval = interval;
            this.tasks = tasks;
        }

        public bool ShouldRun(DateTime currentTime)
        {
            return NextRun < currentTime && LastRun != NextRun;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            foreach (var task in tasks)
            {
                await task.ExecuteAsync(cancellationToken);
            }

            LastRun = NextRun;
            NextRun = LastRun + interval;
        }
    }
}
