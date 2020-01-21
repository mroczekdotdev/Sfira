using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Extensions.DependencyInjection;
using MroczekDotDev.Sfira.Models;

namespace MroczekDotDev.Sfira
{
    public class Startup
    {
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration cfg;

        public Startup(IWebHostEnvironment env, IConfiguration cfg)
        {
            this.env = env;
            this.cfg = cfg;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PostgreSqlDbContext>(options =>
                options.UseNpgsql(cfg.GetConnectionString("PostgreSQL")));

            services.SeedDatabase(cfg.GetSection("Seeding"));

            services.AddRepository();

            services.Configure<FeedOptions>(cfg.GetSection("Feed"));

            services.AddFileUploader(cfg.GetSection("FileUploader"));

            services.AddEmailSender(cfg.GetSection("EmailSender"));

            services.AddCachedStorage(cfg.GetSection("CachedStorage"));

            services.AddJobScheduler(cfg.GetSection("JobScheduler"));

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

            services.AddControllersWithViews();
            services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (env.IsProduction() || env.IsStaging())
            {
                app.UseExceptionHandler("/error");
            }

            app.UseForwardedHeaders();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                    name: "Home.PostsFeed",
                    pattern: "PostsFeed/{count:int?}/{cursor:int?}",
                    new { controller = "Home", action = "PostsFeed" }
                );

                endpoints.MapControllerRoute(
                    name: "Explore",
                    pattern: "Explore/{action}/{count:int?}/{cursor:int?}",
                    new { controller = "Explore", action = "Index" }
                );

                endpoints.MapControllerRoute(
                    name: "Tag",
                    pattern: "Tag/{tagName}/{action}/{count:int?}/{cursor:int?}",
                    new { controller = "Tag", action = "Index" }
                );

                endpoints.MapControllerRoute(
                    name: "Chat",
                    pattern: "Chat/{chatId:int}/{action}",
                    new { controller = "Chat" }
                );

                endpoints.MapControllerRoute(
                   name: "User.DirectChat",
                   pattern: "{userName}/Chat",
                   new { controller = "Chat", action = "DirectChat" }
                );

                endpoints.MapControllerRoute(
                    name: "Post.Comments",
                    pattern: "{postId:int}/Comments/{getCount:bool?}",
                    new { controller = "Comment", action = "Comments" }
                );

                endpoints.MapControllerRoute(
                    name: "Post.CommentsFeed",
                    pattern: "{postId:int}/CommentsFeed/{count:int?}/{cursor:int?}",
                    new { controller = "Comment", action = "CommentsFeed" }
                );

                endpoints.MapControllerRoute(
                    name: "Post.Mark",
                    pattern: "{postId:int}/{interaction}",
                    new { controller = "Post", action = "Mark" }
                );

                endpoints.MapControllerRoute(
                    name: "Default",
                    pattern: "{controller}/{action}/{id?}",
                    new { action = "Index" },
                    new { controller = "(Comment|Home|Messages|Post)" }
                );

                endpoints.MapControllerRoute(
                    name: "User",
                    pattern: "{userName}/{action}/{count:int?}/{cursor:int?}",
                    new { controller = "User", action = "Index" }
                );
            });
        }
    }
}
