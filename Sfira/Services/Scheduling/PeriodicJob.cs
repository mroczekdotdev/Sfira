using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Services.Scheduling
{
    public class PeriodicJob : IScheduledJob
    {
        protected IEnumerable<IScheduledTask> Tasks { get; set; }
        protected TimeSpan Interval { get; set; }
        protected DateTime LastRun { get; set; }
        protected DateTime NextRun { get; set; }

        public PeriodicJob(IEnumerable<IScheduledTask> tasks, DateTime currentTime, TimeSpan interval)
        {
            NextRun = currentTime;
            Interval = interval;
            Tasks = tasks;
        }

        public bool ShouldRun(DateTime currentTime)
        {
            return NextRun < currentTime && LastRun != NextRun;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            foreach (var task in Tasks)
            {
                await task.ExecuteAsync(cancellationToken);
            }

            LastRun = NextRun;
            NextRun = LastRun + Interval;
        }
    }
}
