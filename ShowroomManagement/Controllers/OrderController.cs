using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShowroomManagement.Data;
using ShowroomManagement.Models;

namespace ShowroomManagement.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            var orders = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Vehicle);
            return View(await orders.ToListAsync());
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            // Hiển thị danh sách các xe (Vehicle) và khách hàng (Customer)
            ViewBag.VehicleId = _context.Vehicles.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.Name
            }).ToList();

            ViewBag.CustomerId = _context.Customers.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            ViewBag.PaymentMethods = new List<SelectListItem>
            {
                new SelectListItem { Value = "CreditCard", Text = "Credit Card" },
                new SelectListItem { Value = "Cash", Text = "Cash" },
                new SelectListItem { Value = "BankTransfer", Text = "Bank Transfer" }
            };

            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehicleId,CustomerId,OrderDate,TotalPrice,PaymentMethod")] OrderCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Tạo đơn hàng từ ViewModel
                var order = new Order
                {
                    VehicleId = viewModel.VehicleId,
                    CustomerId = viewModel.CustomerId,
                    OrderDate = viewModel.OrderDate,
                    TotalPrice = viewModel.TotalPrice,
                    PaymentMethod = viewModel.PaymentMethod
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Chuyển hướng đến trang chi tiết đơn hàng
                return RedirectToAction(nameof(Details), new { id = order.Id });
            }

            // Nạp lại dropdown nếu có lỗi
            ViewBag.VehicleId = _context.Vehicles.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.Name
            }).ToList();

            ViewBag.CustomerId = _context.Customers.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            ViewBag.PaymentMethods = new List<SelectListItem>
            {
                new SelectListItem { Value = "CreditCard", Text = "Credit Card" },
                new SelectListItem { Value = "Cash", Text = "Cash" },
                new SelectListItem { Value = "BankTransfer", Text = "Bank Transfer" }
            };

            return View(viewModel);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Orders
                .Include(o => o.Vehicle)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            var viewModel = new OrderCreateViewModel
            {
                VehicleId = order.VehicleId,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                PaymentMethod = order.PaymentMethod,
                Vehicles = _context.Vehicles.Select(v => new SelectListItem
                {
                    Value = v.Id.ToString(),
                    Text = v.Name
                }).ToList(),
                Customers = _context.Customers.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList(),
                PaymentMethods = new List<SelectListItem>
                {
                    new SelectListItem { Value = "CreditCard", Text = "Credit Card" },
                    new SelectListItem { Value = "Cash", Text = "Cash" },
                    new SelectListItem { Value = "BankTransfer", Text = "Bank Transfer" }
                }
            };

            return View(viewModel);
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VehicleId,CustomerId,OrderDate,TotalPrice,PaymentMethod")] OrderCreateViewModel viewModel)
        {
            if (id != viewModel.VehicleId)
                return NotFound();

            if (ModelState.IsValid)
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                    return NotFound();

                // Cập nhật dữ liệu từ ViewModel
                order.VehicleId = viewModel.VehicleId;
                order.CustomerId = viewModel.CustomerId;
                order.OrderDate = viewModel.OrderDate;
                order.TotalPrice = viewModel.TotalPrice;
                order.PaymentMethod = viewModel.PaymentMethod;

                _context.Update(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Vehicle)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
