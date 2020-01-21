using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace MroczekDotDev.Sfira.Services.CachedStorage
{
    public class PopularUsersCached : Cached<ApplicationUser>
    {
        private readonly PostgreSqlDbContext context;

        public PopularUsersCached(PostgreSqlDbContext context, IOptionsMonitor<CachedOptions> optionsAccessor) :
            base(optionsAccessor)
        {
            this.context = context;
        }

        public override ImmutableArray<ApplicationUser> Items { get; set; }

        public override void Reload(int periodInMinutes, int samplesPerMinute)
        {
            DateTime timeBoundary = DateTime.UtcNow - TimeSpan.FromMinutes(periodInMinutes);
            int sampleSize = periodInMinutes * samplesPerMinute;

            var query = context.Posts
                .Where(p => p.PublicationTime > timeBoundary)
                .Where(p => p.CommentsCount > 0 || p.LikesCount > 0 || p.FavoritesCount > 0);

            if (query.Count() < sampleSize)
            {
                query = context.Posts
                    .Where(p => p.CommentsCount > 0 || p.LikesCount > 0 || p.FavoritesCount > 0);
            }

            query = query
                .Include(p => p.Author);

            var grouping = query
                .OrderByDescending(p => p.Id)
                .Take(sampleSize)
                .AsEnumerable()
                .GroupBy(p => p.Author);

            if (grouping.Count() < maxCount)
            {
                grouping = query
                    .AsEnumerable()
                    .GroupBy(p => p.Author);
            }

            Items = grouping
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
