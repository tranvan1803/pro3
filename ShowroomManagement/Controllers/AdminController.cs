using Microsoft.AspNetCore.Mvc;

namespace ShowroomManagement.Controllers
{
    public class AdminController : Controller
    {
        // Action Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
