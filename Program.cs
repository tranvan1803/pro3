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

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{page?}");
});



// Định tuyến mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
