using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShowroomManagement.Controllers;
using ShowroomManagement.Models; // Namespace chứa các ViewModel và ApplicationUser
using System.IO;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager; // Gán RoleManager
    }

    // GET: /Account/Login
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null)
            {
                // Nếu Avatar là NULL thì sử dụng ảnh mặc định
                string avatarUrl = user.Avatar ?? "/images/default-avatar.png";

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Login successful!";
                    return RedirectToLocal(returnUrl);
                }

                TempData["ErrorMessage"] = result.IsLockedOut ? "This account is locked out." : "Invalid login attempt.";
            }
            else
            {
                TempData["ErrorMessage"] = "User not found.";
            }
        }

        return View(model);
    }


    // GET: /Account/Register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // POST: /Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            string avatarPath = "/images/default-avatar.png"; // Mặc định ảnh avatar

            // Kiểm tra nếu người dùng có chọn ảnh đại diện
            if (model.Avatar != null && model.Avatar.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.Avatar.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Avatar.CopyToAsync(stream);
                }

                avatarPath = $"/images/{uniqueFileName}"; // Cập nhật đường dẫn ảnh
            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                Avatar = avatarPath // Lưu đường dẫn ảnh vào Avatar
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Gán vai trò Admin nếu checkbox IsAdmin được chọn
                if (model.IsAdmin)
                {
                    if (!await _roleManager.RoleExistsAsync("Admin"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    }

                    await _userManager.AddToRoleAsync(user, "Admin");
                }

                TempData["SuccessMessage"] = "Registration successful!";
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        TempData["ErrorMessage"] = "Registration failed. Please correct the errors and try again.";
        return View(model);
    }


    // POST: /Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync(); // Sign out
        return RedirectToAction("Index", "Home"); // Redirect to Home page
    }

    // Helper method to handle redirect after login success
    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    // AccessDenied method to handle unauthorized access
    [AllowAnonymous] // Allow all users to access
    public IActionResult AccessDenied()
    {
        return View();
    }
}
