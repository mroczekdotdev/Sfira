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
using MroczekDotDev.Sfira.Services;

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
            services.AddDbContext<SfiraDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("PostgreSQL")));

            services.AddTransient<IDataStorage, EfDataStorage>();

            services.AddFileUploader();

            services.AddEmailSender();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<SfiraDbContext>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddDefaultTokenProviders();

            services.Configure<ForwardedHeadersOptions>(options =>
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)  //remove in 3.0
                .AddRazorPagesOptions(options =>
                {
                    options.AllowAreas = true;
                    options.Conventions.AuthorizeAreaFolder("Account", "/Manage");
                    options.Conventions.AuthorizeAreaPage("Account", "/ConfirmEmail");
                    options.Conventions.AuthorizeAreaPage("Account", "/Logout");
                    options.Conventions.AuthorizeAreaPage("Account", "/Profile");
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/account/login";
                options.LogoutPath = $"/account/logout";
                options.AccessDeniedPath = $"/account/accessdenied";
            });

            services.AddScheduler();
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
                    "Home.PostsFeed",
                    "/PostsFeed/{count:int?}/{cursor:int?}",
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
