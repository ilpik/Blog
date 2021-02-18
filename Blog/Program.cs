using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Blog
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host =  CreateHostBuilder(args).Build();
            try
            {
            var scope = host.Services.CreateScope();

            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await ctx.Database.EnsureCreatedAsync();

            var adminRole = new IdentityRole("Admin");
            if (!ctx.Roles.Any())
            {
                //create role
                await roleManager.CreateAsync(adminRole);
            }

            if(!ctx.Users.Any(u => u.UserName=="admin"))
            {
                //create admin
                var adminUser = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@test.com"
                };
                
                await userManager.CreateAsync(adminUser, "password");

                //add role to user
                
                await userManager.AddToRoleAsync(adminUser, adminRole.Name);

            }

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            await host.RunAsync();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
