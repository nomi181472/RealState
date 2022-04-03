using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using realstate.Data;

[assembly: HostingStartup(typeof(realstate.Areas.Identity.IdentityHostingStartup))]
namespace realstate.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
           /* builder.ConfigureServices((context, services) => {
                services.AddDbContext<realstateContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("realstateContextConnection")));

                services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<realstateContext>();
            });*/
        }
    }
}