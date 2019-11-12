using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Data.Extensions;
using MroczekDotDev.Sfira.Services.CachedStorage;
using MroczekDotDev.Sfira.Services.EmailSender;
using MroczekDotDev.Sfira.Services.FileUploading;
using MroczekDotDev.Sfira.Services.Scheduling;
using System;
using System.Collections.Generic;

namespace MroczekDotDev.Sfira.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddTransient<IRepository, EntityFrameworkRepository>();
            return services;
        }

        public static IServiceCollection SeedDatabase(
            this IServiceCollection services, IConfigurationSection options)
        {
            services.Configure<SeedingOptions>(options);

            var sp = services.BuildServiceProvider();
            var context = sp.GetService<PostgreSqlDbContext>();
            var seedingOptions = sp.GetService<IOptions<SeedingOptions>>();

            context.EnsureSeeded(seedingOptions);

            return services;
        }

        public static IServiceCollection AddEmailSender(
            this IServiceCollection services, IConfigurationSection options)
        {
            services.AddSingleton<IEmailSender, EmailSender>();
            services.Configure<EmailSenderOptions>(options);
            return services;
        }

        public static IServiceCollection AddFileUploader(
            this IServiceCollection services, IConfigurationSection options)
        {
            services.AddSingleton<IFileUploader, FileUploader>();
            services.Configure<FileUploaderOptions>(options);

            return services;
        }

        public static IServiceCollection AddCachedStorage(
            this IServiceCollection services, IConfigurationSection options)
        {
            services.AddSingleton<TrendingTagsCached>();
            services.AddSingleton<PopularUsersCached>();

            services.Configure<CachedOptions>(options.GetSection("Cached"));

            return services;
        }

        public static IServiceCollection AddJobScheduler(
            this IServiceCollection services, IConfigurationSection options)
        {
            services.AddSingleton<TrendingTagsTask>();
            services.AddSingleton<PopularUsersTask>();

            services.AddSingleton<IScheduledJob>(sp =>
            {
                var tasks = new List<IScheduledTask>();
                tasks.Add(sp.GetRequiredService<TrendingTagsTask>());
                tasks.Add(sp.GetRequiredService<PopularUsersTask>());
                return new PopularContentJob(
                    tasks, DateTime.UtcNow, sp.GetRequiredService<IOptionsMonitor<PopularContentOptions>>());
            });

            services.Configure<PopularContentOptions>(options.GetSection("PopularContent"));

            services.AddSingleton<IHostedService, JobSchedulerHostedService>();

            return services;
        }
    }
}
