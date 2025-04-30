using Microsoft.AspNetCore.Identity;
using MyBlog.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.infrastructure
{
    public static class DbIntializer
    {
        public static async Task SeedAdminAsync(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            string adminName = "admin@gmail.com";
            string adminPassword = "Admin@123";
            string adminRole = "Admin";
            // check if role exists
            if(!await roleManager.RoleExistsAsync(adminRole))
                await roleManager.CreateAsync(new IdentityRole {
                    Name = adminRole,
                    NormalizedName = adminRole.ToUpper(),
                });
            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User".ToUpper()
                });
            var admin = await userManager.FindByEmailAsync(adminName);
            if(admin == null)
            {
                // create admin
                admin = new User
                {
                    UserName = adminName,
                    FullName = "Main Admin",
                    Email = adminName,
                    NormalizedEmail = adminName.ToUpper(),
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    // add role to admin
                    await userManager.AddToRoleAsync(admin, adminRole);
                }
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error: {error.Code} - {error.Description}");
                }
            }
        }
    }
}
