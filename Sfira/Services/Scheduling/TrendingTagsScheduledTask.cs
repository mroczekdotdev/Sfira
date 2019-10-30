﻿using Microsoft.Extensions.Logging;
using MroczekDotDev.Sfira.Services.CachedStorage;
using System.Threading;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Services.Scheduling
{
    public class TrendingTagsScheduledTask : IScheduledTask
    {
        private readonly TrendingTagsCached cache;
        private readonly ILogger<TrendingTagsScheduledTask> logger;

        public TrendingTagsScheduledTask(TrendingTagsCached cache, ILogger<TrendingTagsScheduledTask> logger)
        {
            this.cache = cache;
            this.logger = logger;
        }

        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cache.Reload();
            return Task.CompletedTask;
        }
    }
}
