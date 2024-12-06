using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ShowroomManagement.Data;
using ShowroomManagement.Models;
using X.PagedList.Extensions;
using System.IO;

namespace ShowroomManagement.Controllers
{
    public class VehicleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehicleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vehicle
        public IActionResult Index(int? page, string searchTerm)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var vehicles = _context.Vehicles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                vehicles = vehicles.Where(v => (v.Name ?? "").Contains(searchTerm) || (v.Brand ?? "").Contains(searchTerm));
            }

            var pagedVehicles = vehicles.OrderBy(v => v.Name).ToPagedList(pageNumber, pageSize);
            return View(pagedVehicles);
        }

        // GET: Vehicle/Details/5
        public IActionResult Details(int id)
        {
            var vehicle = _context.Vehicles.FirstOrDefault(v => v.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle); // Đảm bảo truyền đúng kiểu `Vehicle`
        }


        // GET: Vehicle/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehicle/Create
        // POST: Vehicle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleViewModel model)
        {
            if (ModelState.IsValid)
            {
                string? imagePath = null;

                if (model.Image != null && model.Image.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/vehicles");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.Image.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }

                    imagePath = $"/images/vehicles/{uniqueFileName}";
                }

#pragma warning disable CS8601 // Possible null reference assignment.
                var vehicle = new Vehicle
                {
                    Name = model.Name,
                    Brand = model.Brand,
                    ModelNo = model.ModelNo,
                    Price = model.Price,
                    Status = model.Status,
                    ImagePath = imagePath  // Lưu đường dẫn ảnh vào DB
                };
#pragma warning restore CS8601 // Possible null reference assignment.

                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        // POST: Vehicle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, ModelNo, Name, Brand, Price, Status")] Vehicle vehicle)
        {
            if (id != vehicle.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Vehicle updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }


        // GET: Vehicle/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null) return NotFound();

            return View(vehicle);
        }

        // POST: Vehicle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Vehicle deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Vehicle not found!";
            }

            return RedirectToAction(nameof(Index));
        }


        // Báo cáo tồn kho
        public IActionResult InventoryReport(string status = "All")
        {
            var inventory = _context.Vehicles.AsQueryable();

            if (status != "All")
            {
                inventory = inventory.Where(v => v.Status == status);
            }

            return View(inventory.ToList());
        }

        // Xuất báo cáo tồn kho ra Excel
        public IActionResult ExportInventory()
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Inventory Report");
                var vehicles = _context.Vehicles.ToList();

                worksheet.Cells[1, 1].Value = "Model No";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Brand";
                worksheet.Cells[1, 4].Value = "Price";
                worksheet.Cells[1, 5].Value = "Status";

                for (int i = 0; i < vehicles.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = vehicles[i].ModelNo;
                    worksheet.Cells[i + 2, 2].Value = vehicles[i].Name;
                    worksheet.Cells[i + 2, 3].Value = vehicles[i].Brand;
                    worksheet.Cells[i + 2, 4].Value = vehicles[i].Price;
                    worksheet.Cells[i + 2, 5].Value = vehicles[i].Status;
                }

                var stream = new MemoryStream(package.GetAsByteArray());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "InventoryReport.xlsx");
            }
        }

        // Dữ liệu biểu đồ tồn kho
        [HttpGet]
        public JsonResult GetInventoryData()
        {
            var inventoryData = _context.Vehicles
                .GroupBy(v => v.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                }).ToList();

            return Json(inventoryData);
        }

        // Trang biểu đồ tồn kho
        public IActionResult InventoryChart()
        {
            return View();
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.Id == id);
        }
    }
}
