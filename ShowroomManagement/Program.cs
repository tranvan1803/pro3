using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShowroomManagement.Data; // Namespace chứa ApplicationDbContext

var builder = WebApplication.CreateBuilder(args);

// Cấu hình DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 31)) // Đảm bảo phiên bản MySQL đúng
    ));

// Cấu hình Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Cho phép đăng nhập mà không cần xác nhận email
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Cấu hình Authentication với Cookie
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Đường dẫn đến trang đăng nhập
    options.LogoutPath = "/Account/Logout"; // Đường dẫn đăng xuất
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Thời gian hiệu lực cookie
});

// Đăng ký dịch vụ Controller với Views
builder.Services.AddControllersWithViews();

// Cấu hình Authorization
builder.Services.AddAuthorization(options =>
{
    // Có thể thêm các policy nếu cần
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

var app = builder.Build();

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

app.MapControllerRoute(
    name: "access-denied",
    pattern: "Account/AccessDenied",
    defaults: new { controller = "Account", action = "AccessDenied" });

app.Run();
