using Microsoft.Extensions.Options;
using MroczekDotDev.Sfira.Data;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace MroczekDotDev.Sfira.Services.CachedStorage
{
    public class TrendingTagsCached : Cached<string>
    {
        private readonly PostgreSqlDbContext context;
        private TimeSpan period;
        private int sampleSize;

        public TrendingTagsCached(PostgreSqlDbContext context, IOptionsMonitor<CachedOptions> optionsAccessor) :
            base(optionsAccessor)
        {
            this.context = context;
        }

        public override ImmutableArray<string> Items { get; set; }

        public override void Reload(int periodInMinutes, int samplesPerMinute)
        {
            period = TimeSpan.FromMinutes(periodInMinutes);
            sampleSize = periodInMinutes * samplesPerMinute;

            var query = context.Posts
                .Where(p => p.PublicationTime > DateTime.UtcNow - period);

            if (query.Count() < sampleSize)
            {
                query = context.Posts;
            }

            var groupingQuery = query
                .OrderByDescending(p => p.Id)
                .Take(sampleSize)
                .Select(p => p.Tags)
                .SelectMany(pt => pt.Split())
                .Where(t => t != string.Empty)
                .GroupBy(t => t.ToLower());

            if (groupingQuery.Count() < maxCount)
            {
                groupingQuery = query
                    .Select(p => p.Tags)
                    .SelectMany(pt => pt.Split())
                    .Where(t => t != string.Empty)
                    .GroupBy(t => t.ToLower());
            }

            Items = groupingQuery
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(maxCount)
                .ToImmutableArray();
        }
    }
}
