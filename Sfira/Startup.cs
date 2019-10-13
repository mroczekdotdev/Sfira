using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MroczekDotDev.Sfira.Data;

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
            services.Configure<ForwardedHeadersOptions>(options =>
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddDbContext<SfiraDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("PostgreSQL")));

            services.AddTransient<IDataStorage, EfDataStorage>();

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
            app.UseForwardedHeaders();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "Follow",
                    "{userName}/follow",
                    new { controller = "User", action = "Follow" }
                );

                routes.MapRoute(
                    "Unfollow",
                    "{userName}/unfollow",
                    new { controller = "User", action = "Unfollow" }
                );

                routes.MapRoute(
                    "Followers",
                    "{userName}/followers",
                    new { controller = "User", action = "Followers" }
                );

                routes.MapRoute(
                    "Media",
                    "{userName}/media",
                    new { controller = "User", action = "Media" }
                );

                routes.MapRoute(
                    "Comments",
                    "{postId}/comments",
                    new { controller = "Comment", action = "Feed" }
                );

                routes.MapRoute(
                    "Mark",
                    "{postId}/{interaction}",
                    new { controller = "Post", action = "Mark" }
                );

                routes.MapRoute(
                    "DirectChat",
                    "{userName}/chat",
                    new { controller = "Chat", action = "DirectChat" }
                );

                routes.MapRoute(
                    "Message",
                    "chat/{chatId}/createmessage",
                    new { controller = "Chat", action = "CreateMessage" }
                );

                routes.MapRoute(
                     "Messages",
                     "chat/{chatId}/messagesfeed",
                     new { controller = "Chat", action = "MessagesFeed" }
                 );

                routes.MapRoute(
                    "Tag",
                    "tag/{tagName}",
                    new { controller = "Tag", action = "Index" }
                );

                routes.MapRoute(
                    "Default",
                    "{controller}/{action}/{id?}",
                    new { controller = "Home", action = "Index" },
                    new { controller = "(attachment|chat|comment|explore|home|identity|messages|post|search)" }
                );

                routes.MapRoute(
                    "User",
                    "{userName}",
                    new { controller = "User", action = "Index" }
                );
            });
        }
    }
}
