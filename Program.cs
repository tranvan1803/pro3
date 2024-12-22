using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShowroomManagement.Data; // Namespace chứa ApplicationDbContext
using ShowroomManagement.Models; // Namespace chứa ApplicationUser

var builder = WebApplication.CreateBuilder(args);

// Cấu hình DbContext cho MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 31)) // Đảm bảo phiên bản MySQL đúng
    ));

// Cấu hình Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Cho phép đăng nhập mà không cần xác nhận email
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Cấu hình Cookie Authentication (đã được cấu hình bởi AddIdentity)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Đường dẫn đến trang đăng nhập
    options.LogoutPath = "/Account/Logout"; // Đường dẫn đăng xuất
    options.AccessDeniedPath = "/Account/AccessDenied"; // Đường dẫn khi bị từ chối truy cập
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Thời gian hiệu lực cookie
});

// Đăng ký dịch vụ Controller với Views
builder.Services.AddControllersWithViews();

// Cấu hình Authorization (có thể thêm policy nếu cần)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin")); // Policy chỉ dành cho Admin
    options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer")); // Policy chỉ dành cho Customer
});

// Tạo và gán vai trò mặc định (Admin và Customer)
async Task CreateRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // Tạo các vai trò nếu chưa có
    string[] roles = { "Admin", "Customer" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Gán vai trò Admin cho tài khoản mặc định
    var adminUser = await userManager.FindByEmailAsync("admin@showroom.com");
    if (adminUser == null)
    {
        var newAdmin = new ApplicationUser
        {
            UserName = "admin",
            Email = "admin@showroom.com",
            Avatar = "/images/default-avatar.png"
        };
        var result = await userManager.CreateAsync(newAdmin, "Admin@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdmin, "Admin");
        }
    }
}

var app = builder.Build();

// Tạo vai trò và gán tài khoản mặc định
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await CreateRoles(services);
}

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Thêm Authentication và Authorization vào pipeline
app.UseAuthentication();
app.UseAuthorization();

// Định tuyến mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
