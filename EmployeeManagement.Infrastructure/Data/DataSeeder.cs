using EmployeeManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new IdentityRole("Admin"));

        if (!await roleManager.RoleExistsAsync("Employee"))
            await roleManager.CreateAsync(new IdentityRole("Employee"));

        var adminEmail = "admin@company.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "System",
                LastName = "Admin"
            };
            var result = await userManager.CreateAsync(adminUser, "Admin123!");

            if (result.Succeeded)
                await userManager.AddToRoleAsync(adminUser, "Admin");
        }
        
        var checkAdmin = await userManager.FindByEmailAsync(adminEmail);
        if (checkAdmin != null)
        {
            Console.WriteLine($"Admin exists: {checkAdmin.Email}");
        }

    }
}
