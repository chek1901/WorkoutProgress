using System;
using AWO.Data;
using AwoAppServices.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AWO
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }


    //public static void Main(string[] args)
    //    {
    //        var iHost = CreateHostBuilder(args).Build();
    //        InitializeDatabase(iHost);
    //        iHost.Run();
    //    }
    //    private static void InitializeDatabase(IHost iHost)
    //    {
    //        using (var scope = iHost.Services.CreateScope())
    //        {
    //            try
    //            {
    //                var services = scope.ServiceProvider;
    //                var context = services.GetRequiredService<GymadminContext>();
    //                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    //                var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
    //                var seeding = new DatabaseInitializer();
    //                seeding.Initialize(context, userManager, roleManager).Wait();


    //            }
    //            catch (Exception ex)
    //            {
    //                Console.WriteLine(ex.Message);
    //                throw;
    //            }
    //        }
    //    }

    //    public static IHostBuilder CreateHostBuilder(string[] args) =>
    //        Host.CreateDefaultBuilder(args)
    //            .ConfigureWebHostDefaults(webBuilder =>
    //            {
    //                webBuilder.UseStartup<Startup>();
    //            });
    //}
}
