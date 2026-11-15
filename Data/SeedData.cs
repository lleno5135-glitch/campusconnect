using CampusConnect.Models;
using Microsoft.AspNetCore.Identity;

namespace CampusConnect.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var roles = services.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in new[] { "Admin", "Student" })
            if (!await roles.RoleExistsAsync(role))
                await roles.CreateAsync(new IdentityRole(role));

        var users = services.GetRequiredService<UserManager<ApplicationUser>>();
        const string adminEmail = "admin@campusconnect.local";
        var admin = await users.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true, FullName = "Campus Admin" };
            var result = await users.CreateAsync(admin, "Campus@123");
            if (result.Succeeded) await users.AddToRoleAsync(admin, "Admin");
        }
    }
}
