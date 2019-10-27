using System.Collections.Generic;

namespace MroczekDotDev.Sfira.Services.Scheduling
{
    public class PeriodicScheduledJobBuilder
    {
        public IList<IScheduledTask> Tasks { get; }

        public void AddTask(IScheduledTask task)
        {
            Tasks.Add(task);
        }
    }
}
