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
