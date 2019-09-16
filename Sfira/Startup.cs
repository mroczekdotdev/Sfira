﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MroczekDotDev.Sfira.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MroczekDotDev.Sfira
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDataStorage, EfDataStorage>();

            services.AddDbContext<SfiraDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("PostgreSQL")));

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2); //remove in 3.0
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (env.IsProduction() || env.IsStaging())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "Mark",
                    "{postId}/{interaction}",
                    new { controller = "Post", action = "Mark" }
                );

                routes.MapRoute(
                    "Comments",
                    "comments/{postId}",
                    new { controller = "Comment", action = "GetCommentsByPostId" }
                );

                routes.MapRoute(
                    "Tag",
                    "tag/{id}",
                    new { controller = "Tag", action = "GetPostsByTag" }
                );

                routes.MapRoute(
                    "Default",
                    "{controller}/{action}/{id?}",
                    new { controller = "Home", action = "Index" },
                    new { controller = "(attachment|comment|explore|home|identity|messages|post|search)" }
                );

                routes.MapRoute(
                    "User",
                    "{userName}",
                    new { controller = "User", action = "GetUserByUserName" }
                );
            });
        }
    }
}
