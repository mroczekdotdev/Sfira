using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace MroczekDotDev.Sfira.Services.Scheduling
{
    public class PopularContentJob : PeriodicJob
    {
        public PopularContentJob(
            IEnumerable<IScheduledTask> tasks,
            DateTime currentTime,
            IOptionsMonitor<PopularContentOptions> optionsAccessor) :
            base(tasks, currentTime, TimeSpan.FromMinutes(optionsAccessor.CurrentValue.IntervalInMinutes))
        {
        }
    }
}
