using MroczekDotDev.Sfira.Data;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace MroczekDotDev.Sfira.Services.CachedStorage
{
    public class TrendingTagsCached : Cached<string>
    {
        private const int samplesPerMinute = 600;
        private readonly PostgreSqlDbContext context;
        private readonly TimeSpan period = TimeSpan.FromMinutes(60);
        private readonly int sampleSize;

        public TrendingTagsCached(PostgreSqlDbContext context)
        {
            this.context = context;
            sampleSize = (int)period.TotalMinutes * samplesPerMinute;
        }

        public override ImmutableArray<string> items { get; set; }

        public override void Reload()
        {
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

            items = groupingQuery
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(maxCount)
                .ToImmutableArray();
        }
    }
}
