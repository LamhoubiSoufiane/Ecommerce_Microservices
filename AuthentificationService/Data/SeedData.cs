using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace AuthentificationService.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Création des rôles
            string[] roleNames = { "Admin", "Manager", "User" };
            
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Création d'un admin par défaut
            var adminUser = await userManager.FindByEmailAsync("lamhoubi2000@gmail.com");
            if (adminUser == null)
            {
                var admin = new IdentityUser
                {
                    UserName = "lamhoubisoufiane",
                    Email = "lamhoubi2000@gmail.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Admin@123456");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
            
        }
    }
}
