using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Extensions.DependencyInjection;
using MroczekDotDev.Sfira.Models;

namespace MroczekDotDev.Sfira
{
    public class Startup
    {
        private readonly IHostingEnvironment environment;
        private readonly IConfiguration configuration;

        public Startup(IHostingEnvironment environment, IConfiguration configuration)
        {
            this.environment = environment;
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PostgreSqlDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));

            services.SeedDatabase(configuration.GetSection("Seeding"));

            services.AddRepository();

            services.Configure<FeedOptions>(configuration.GetSection("Feed"));

            services.AddFileUploader(configuration.GetSection("FileUploader"));

            services.AddEmailSender(configuration.GetSection("EmailSender"));

            services.AddCachedStorage(configuration.GetSection("CachedStorage"));

            services.AddJobScheduler(configuration.GetSection("JobScheduler"));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<PostgreSqlDbContext>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddDefaultTokenProviders();

            services.Configure<ForwardedHeadersOptions>(options =>
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorPagesOptions(options =>
                {
                    options.AllowAreas = true;
                    options.Conventions.AuthorizeAreaPage("Account", "/Index");
                    options.Conventions.AuthorizeAreaPage("Account", "/ChangePassword");
                    options.Conventions.AuthorizeAreaPage("Account", "/CloseAccount");
                    options.Conventions.AuthorizeAreaPage("Account", "/ConfirmEmail");
                    options.Conventions.AuthorizeAreaPage("Account", "/Logout");
                    options.Conventions.AuthorizeAreaPage("Account", "/Profile");
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (environment.IsProduction() || environment.IsStaging())
            {
                app.UseExceptionHandler("/error");
            }

            app.UseStatusCodePages();
            app.UseForwardedHeaders();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "Home.PostsFeed",
                    "PostsFeed/{count:int?}/{cursor:int?}",
                    new { controller = "Home", action = "PostsFeed" }
                );

                routes.MapRoute(
                    "Explore",
                    "Explore/{action}/{count:int?}/{cursor:int?}",
                    new { controller = "Explore", action = "Index" }
                );

                routes.MapRoute(
                    "Tag",
                    "Tag/{tagName}/{action}/{count:int?}/{cursor:int?}",
                    new { controller = "Tag", action = "Index" }
                );

                routes.MapRoute(
                    "Chat",
                    "Chat/{chatId:int}/{action}",
                    new { controller = "Chat" }
                );

                routes.MapRoute(
                   "User.DirectChat",
                   "{userName}/Chat",
                   new { controller = "Chat", action = "DirectChat" }
                );

                routes.MapRoute(
                    "Post.CommentsFeed",
                    "{postId:int}/Comments",
                    new { controller = "Comment", action = "Feed" }
                );

                routes.MapRoute(
                    "Post.Mark",
                    "{postId:int}/{interaction}",
                    new { controller = "Post", action = "Mark" }
                );

                routes.MapRoute(
                    "Default",
                    "{controller}/{action}/{id?}",
                    new { action = "Index" },
                    new { controller = "(Comment|Home|Messages|Post)" }
                );

                routes.MapRoute(
                    "User",
                    "{userName}/{action}/{count:int?}/{cursor:int?}",
                    new { controller = "User", action = "Index" }
                );
            });
        }
    }
}
