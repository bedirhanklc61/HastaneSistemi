using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using HastaneSistemi.Models;

namespace HastaneSistemi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            SeedDefaultAdmin(host);
            host.Run();
        }

        private static void SeedDefaultAdmin(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<HastaneDbContext>();
                if (!context.Adminler.Any())
                {
                    var hasher = new PasswordHasher<AdminBilgileri>();
                    var admin = new AdminBilgileri { Email = "admin@hastane.com" };
                    admin.Sifre = hasher.HashPassword(admin, "Admin123!");
                    context.Adminler.Add(admin);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Default admin seeding failed");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
