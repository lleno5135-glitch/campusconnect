using CampusConnect.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace CampusConnect.Data;

public static class SeedData
{
    public static async Task InitializeAsync(
        IServiceProvider services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var roles = services.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in new[] { "Admin", "Student" })
            if (!await roles.RoleExistsAsync(role))
                await roles.CreateAsync(new IdentityRole(role));

        var adminEmail = environment.IsDevelopment()
            ? "admin@campusconnect.local"
            : configuration["SeedAdmin:Email"];
        var adminPassword = environment.IsDevelopment()
            ? "Campus@123"
            : configuration["SeedAdmin:Password"];
        if (string.IsNullOrWhiteSpace(adminEmail) && string.IsNullOrWhiteSpace(adminPassword))
            return;
        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
            throw new InvalidOperationException("Configure both SeedAdmin:Email and SeedAdmin:Password to seed a production administrator.");

        var users = services.GetRequiredService<UserManager<ApplicationUser>>();
        var admin = await users.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true, FullName = "Campus Admin" };
            var result = await users.CreateAsync(admin, adminPassword);
            if (!result.Succeeded)
                throw new InvalidOperationException($"Could not create the configured administrator: {string.Join("; ", result.Errors.Select(error => error.Description))}");
            var roleResult = await users.AddToRoleAsync(admin, "Admin");
            if (!roleResult.Succeeded)
                throw new InvalidOperationException($"Could not assign the administrator role: {string.Join("; ", roleResult.Errors.Select(error => error.Description))}");
        }
    }
}
