using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShowroomManagement.Data;
using ShowroomManagement.Models;

namespace ShowroomManagement.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }


    [Authorize]
    public IActionResult Index()
    {
        var data = _context.Vehicles.ToList(); // Lấy danh sách xe từ cơ sở dữ liệu
        if (data == null || data.Count == 0)
        {
            data = new List<Vehicle>(); // Truyền danh sách trống nếu không có dữ liệu
        }
        return View(data);
    }



    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize]
    public class VehicleController : Controller
    {

    }
     public IActionResult CreateVehicle()
    {
        var vehicle = new Vehicle(); // Tạo một đối tượng Vehicle mới
        return View(vehicle); // Trả về view với mô hình Vehicle
    }
}
