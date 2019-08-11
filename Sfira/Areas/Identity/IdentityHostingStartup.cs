using System;
using MarcinMroczek.Sfira.Data;
using MarcinMroczek.Sfira.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(MarcinMroczek.Sfira.Areas.Identity.IdentityHostingStartup))]
namespace MarcinMroczek.Sfira.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDefaultIdentity<ApplicationUser>(options => {
                    options.User.RequireUniqueEmail = true;
                })
                    .AddEntityFrameworkStores<SfiraDbContext>();
            });
        }
    }
}