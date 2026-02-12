
using Microsoft.AspNetCore.Identity;

namespace GBMovieRentalSite.Areas.Identity.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Definiera roller
            string[] roleNames = { "Admin", "User" };

            // Skapa roller om de inte finns
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Skapa en admin-användare om den inte finns
            var adminEmail = "admin@gbmovierental.se";
            var adminUserName = "admin";
            var adminPassword = "Admin123!"; // Byt till ett säkert lösenord!

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var createAdmin = await userManager.CreateAsync(newAdmin, adminPassword);
                if (createAdmin.Succeeded)
                {
                    // Lägg till admin-användaren i Admin-rollen
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}
