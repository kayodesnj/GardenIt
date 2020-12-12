// using System;
// using GardenIt.Areas.Identity.Data;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Identity.UI;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;

// [assembly: HostingStartup(typeof(GardenIt.Areas.Identity.IdentityHostingStartup))]
// namespace GardenIt.Areas.Identity
// {
//     public class IdentityHostingStartup : IHostingStartup
//     {
//         public void Configure(IWebHostBuilder builder)
//         {
//             builder.ConfigureServices((context, services) => {
//                 services.AddDbContext<GardenItIdentityDbContext>(options =>
//                     options.UseSqlServer(
//                         context.Configuration.GetConnectionString("GardenItIdentityDbContextConnection")));

//                 services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//                     .AddEntityFrameworkStores<GardenItIdentityDbContext>();
//             });
//         }
//     }
// }