using Microsoft.AspNetCore.Identity;

public static class UserInitializer
{
    public static async Task SeedAdmin(UserManager<IdentityUser> userManager)
    {
        var adminEmail = "admin@showroom.com";
        var adminPassword = "Admin@123";

        
        {
            var admin = new IdentityUser { UserName = adminEmail, Email = adminEmail };
            await userManager.CreateAsync(admin, adminPassword);
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}
