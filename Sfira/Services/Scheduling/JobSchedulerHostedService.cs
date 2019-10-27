using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Services.Scheduling
{
    public class JobSchedulerHostedService : IHostedService
    {
        private readonly IEnumerable<IScheduledJob> jobs;

        public JobSchedulerHostedService(IEnumerable<IScheduledJob> jobs)
        {
            this.jobs = jobs;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task executingTask = ExecuteAsync(cts.Token);
            return executingTask.IsCompleted ? executingTask : Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await ExecuteOnceAsync(cancellationToken);
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            }
        }

        private async Task ExecuteOnceAsync(CancellationToken cancellationToken)
        {
            var taskFactory = new TaskFactory(TaskScheduler.Current);
            DateTime currentTime = DateTime.UtcNow;

            var jobsToRun = jobs
                .Where(t => t.ShouldRun(currentTime))
                .ToArray();

            foreach (var job in jobsToRun)
            {
                await taskFactory.StartNew(
                    async () => await job.Run(cancellationToken),
                    cancellationToken);
            }
        }
    }
}
