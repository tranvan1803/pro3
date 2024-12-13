using Microsoft.AspNetCore.Identity;

public static class RoleInitializer
{
    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("Employee"))
        {
            await roleManager.CreateAsync(new IdentityRole("Employee"));
        }
    }
}
