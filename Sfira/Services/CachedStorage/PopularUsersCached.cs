using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace MroczekDotDev.Sfira.Services.CachedStorage
{
    public class PopularUsersCached : Cached<ApplicationUser>
    {
        private const int samplesPerMinute = 600;
        private readonly PostgreSqlDbContext context;
        private readonly TimeSpan period = TimeSpan.FromMinutes(60);
        private readonly int sampleSize;

        public PopularUsersCached(PostgreSqlDbContext context)
        {
            this.context = context;
            sampleSize = (int)period.TotalMinutes * samplesPerMinute;
        }

        public override ImmutableArray<ApplicationUser> items { get; set; }

        public override void Reload()
        {
            var query = context.Posts
                .Where(p => p.PublicationTime > DateTime.UtcNow - period)
                .Where(p => p.CommentsCount > 0 || p.LikesCount > 0 || p.FavoritesCount > 0);

            if (query.Count() < sampleSize)
            {
                query = context.Posts
                    .Where(p => p.CommentsCount > 0 || p.LikesCount > 0 || p.FavoritesCount > 0);
            }

            var groupingQuery = query
                .OrderByDescending(p => p.Id)
                .Take(sampleSize)
                .GroupBy(p => p.Author);

            if (groupingQuery.Count() < maxCount)
            {
                groupingQuery = query
                    .GroupBy(p => p.Author);
            }

            items = groupingQuery
                .Select(g => new
                {
                    Author = g.Key,
                    Popularity = g.Sum(p => p.CommentsCount + p.LikesCount + p.FavoritesCount)
                })
               .OrderByDescending(g => g.Popularity)
               .Select(g => g.Author)
               .Take(maxCount)
               .ToImmutableArray();
        }
    }
}
