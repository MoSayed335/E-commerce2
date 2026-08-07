using ECommerce2.Models;
using Microsoft.AspNetCore.Identity;

namespace ECommerce2.DataAccess
{
    public static class DataSeeder
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var context = serviceProvider.GetRequiredService<AppDbContext>();

            // Seed Roles
            string[] roleNames = { "Admin", "Customer" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Seed Admin User
            string adminEmail = "admin@admin.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FName = "System",
                    LName = "Admin",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }

            // Seed Default Governorate
            if (!context.Governorates.Any())
            {
                context.Governorates.Add(new Governorate
                {
                    NameAr = "القاهرة",
                    NameEn = "Cairo",
                    Fee = 50,
                    EstimatedDelivery = "1-2 Business Days"
                });
                await context.SaveChangesAsync();
            }

            // Seed Store Settings
            if (!context.StoreSettings.Any())
            {
                context.StoreSettings.Add(new StoreSetting { Key = "FreeShippingThreshold", Value = "1000" });
                await context.SaveChangesAsync();
            }
        }
    }
}
