using System.Threading;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Services.Scheduling
{
    public interface IScheduledTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
