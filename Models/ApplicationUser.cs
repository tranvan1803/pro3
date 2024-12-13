using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string Avatar { get; set; } = "/images/default-avatar.png"; // Đường dẫn mặc định
}
