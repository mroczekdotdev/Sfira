using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MroczekDotDev.Sfira.Services;
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
            return services.AddSingleton<IEmailSender, EmailSender>();
        }

        public static IServiceCollection AddFileUploader(this IServiceCollection services)
        {
            return services.AddSingleton<IFileUploader, FileUploader>();
        }

        public static IServiceCollection AddScheduler(this IServiceCollection services)
        {
            {
                services.AddSingleton<TrendingTagsScheduledTask>();
                services.AddSingleton<TrendingUsersScheduledTask>();

                services.AddSingleton<IScheduledJob>(sp =>
                {
                    var tasks = new List<IScheduledTask>();
                    tasks.Add(sp.GetRequiredService<TrendingTagsScheduledTask>());
                    tasks.Add(sp.GetRequiredService<TrendingUsersScheduledTask>());
                    return new PeriodicScheduledJob(tasks, DateTime.UtcNow, TimeSpan.FromSeconds(10));
                });

                return services.AddSingleton<IHostedService, JobSchedulerHostedService>();
            }
        }
    }
}
