using System;
using System.Threading;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Services.Scheduling
{
    public interface IScheduledJob
    {
        bool ShouldRun(DateTime currentTime);

        Task Run(CancellationToken cancellationToken);
    }
}
