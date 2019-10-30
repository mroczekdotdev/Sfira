using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MroczekDotDev.Sfira.Services;
using MroczekDotDev.Sfira.Services.CachedStorage;
using MroczekDotDev.Sfira.Services.FileUploading;
using MroczekDotDev.Sfira.Services.Scheduling;
using System;
using System.Collections.Generic;

namespace MroczekDotDev.Sfira.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddEmailSender(this IServiceCollection services)
        {
            services.AddSingleton<IEmailSender, EmailSender>();
            return services;
        }

        public static IServiceCollection AddFileUploader(this IServiceCollection services)
        {
            services.AddSingleton<IFileUploader, FileUploader>();
            return services;
        }

        public static IServiceCollection AddCachedStorage(this IServiceCollection services)
        {
            services.AddSingleton<TrendingTagsCached>();
            services.AddSingleton<PopularUsersCached>();
            return services;
        }

        public static IServiceCollection AddScheduler(this IServiceCollection services)
        {
            services.AddSingleton<TrendingTagsScheduledTask>();
            services.AddSingleton<PopularUsersScheduledTask>();

            services.AddSingleton<IScheduledJob>(sp =>
            {
                var tasks = new List<IScheduledTask>();
                tasks.Add(sp.GetRequiredService<TrendingTagsScheduledTask>());
                tasks.Add(sp.GetRequiredService<PopularUsersScheduledTask>());
                return new PeriodicScheduledJob(tasks, DateTime.UtcNow, TimeSpan.FromMinutes(60));
            });

            services.AddSingleton<IHostedService, JobSchedulerHostedService>();

            return services;
        }
    }
}
