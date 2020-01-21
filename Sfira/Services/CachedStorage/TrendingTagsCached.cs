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

        public TrendingTagsCached(PostgreSqlDbContext context, IOptionsMonitor<CachedOptions> optionsAccessor) :
            base(optionsAccessor)
        {
            this.context = context;
        }

        public override ImmutableArray<string> Items { get; set; }

        public override void Reload(int periodInMinutes, int samplesPerMinute)
        {
            DateTime timeBoundary = DateTime.UtcNow - TimeSpan.FromMinutes(periodInMinutes);
            int sampleSize = periodInMinutes * samplesPerMinute;

            var query = context.Posts
                .Where(p => p.PublicationTime > timeBoundary);

            if (query.Count() < sampleSize)
            {
                query = context.Posts;
            }

            var grouping = query
                .OrderByDescending(p => p.Id)
                .Take(sampleSize)
                .Select(p => p.Tags)
                .AsEnumerable()
                .SelectMany(pt => pt.Split())
                .Where(t => !string.IsNullOrEmpty(t))
                .GroupBy(t => t.ToLower());

            if (grouping.Count() < maxCount)
            {
                grouping = query
                    .Select(p => p.Tags)
                    .AsEnumerable()
                    .SelectMany(pt => pt.Split())
                    .Where(t => t != string.Empty)
                    .GroupBy(t => t.ToLower());
            }

            Items = grouping
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(maxCount)
                .ToImmutableArray();
        }
    }
}
