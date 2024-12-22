using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShowroomManagement.Data;
using ShowroomManagement.Models;
using System.Security.Claims;

namespace ShowroomManagement.Controllers
{
    public class CustomerHomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerHomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CustomerHome
        public async Task<IActionResult> Index()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            return View(vehicles);
        }

        // GET: CustomerHome/Order
        public IActionResult Order(int vehicleId)
        {
            var vehicle = _context.Vehicles.FirstOrDefault(v => v.Id == vehicleId);
            if (vehicle == null)
            {
                return NotFound();
            }

            var viewModel = new OrderCreateViewModel
            {
                VehicleId = vehicle.Id,
                VehicleName = vehicle.Name,
                Price = vehicle.Price
            };

            return View(viewModel);
        }

        // POST: CustomerHome/Order
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(OrderCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Lấy CustomerId từ Claims
                var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "CustomerId")?.Value;

                if (!int.TryParse(customerIdClaim, out int customerId))
                {
                    return BadRequest("Invalid Customer ID.");
                }

                // Tạo đơn hàng
                var order = new Order
                {
                    VehicleId = model.VehicleId,
                    CustomerId = customerId,
                    OrderDate = DateTime.Now,
                    Quantity = model.Quantity,
                    TotalPrice = model.Price * model.Quantity,
                    PaymentMethod = model.PaymentMethod
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Order placed successfully!";
                return RedirectToAction("Invoice", new { orderId = order.Id });
            }

            TempData["ErrorMessage"] = "Failed to place order.";
            return View(model);
        }

        // GET: CustomerHome/Invoice
        public async Task<IActionResult> Invoice(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Vehicle)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
