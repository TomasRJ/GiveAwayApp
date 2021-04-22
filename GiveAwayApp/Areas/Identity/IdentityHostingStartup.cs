using System;
using GiveAwayApp.Areas.Identity.Data;
using GiveAwayApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(GiveAwayApp.Areas.Identity.IdentityHostingStartup))]
namespace GiveAwayApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            /*builder.ConfigureServices((context, services) => {
                services.AddDbContext<GiveAwayAppContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("GiveAwayAppContextConnection")));

                services.AddDefaultIdentity<GiveAwayAppUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<GiveAwayAppContext>();
            });*/
        }
    }
}