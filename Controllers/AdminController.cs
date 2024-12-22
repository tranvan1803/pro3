using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShowroomManagement.Controllers
{
    [Authorize(Policy = "AdminOnly")] // Chỉ Admin có quyền truy cập
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        // Các chức năng quản lý khác (Manage Orders, Manage Vehicles, etc.)
        public IActionResult ManageOrders()
        {
            return View();
        }

        public IActionResult ManageVehicles()
        {
            return View();
        }

        public IActionResult ManageCustomers()
        {
            return View();
        }
    }
}
