using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShowroomManagement.Data;
using ShowroomManagement.Models;

namespace ShowroomManagement.Controllers
{
    [Authorize]
    public class SettingController : Controller
    {
        
        private readonly ApplicationDbContext _context;

        public SettingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var settings = _context.Settings.ToList();
            return View(settings);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Setting setting)
        {
            if (ModelState.IsValid)
            {
                _context.Settings.Add(setting);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(setting);
        }

        // GET: Setting/Edit/5
        public IActionResult Edit(int id)
        {
            var setting = _context.Settings.Find(id);
            if (setting == null)
            {
                return NotFound(); // Nếu không tìm thấy Setting theo ID
            }

            return View(setting); // Trả về view với đối tượng setting
        }

        // POST: Setting/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Setting setting)
        {
            if (id != setting.Id)
            {
                return NotFound(); // Nếu ID không khớp, trả về lỗi
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(setting); // Cập nhật Setting
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Setting updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SettingExists(setting.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index)); // Quay lại trang danh sách
            }
            return View(setting); // Trả về form chỉnh sửa nếu có lỗi
        }

        // GET: Setting/Delete/5
        public IActionResult Delete(int id)
        {
            var setting = _context.Settings.Find(id);
            if (setting == null)
            {
                TempData["ErrorMessage"] = "Setting not found!";
                return RedirectToAction(nameof(Index));
            }

            return View(setting); // Optional: Return a confirmation page for delete.
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var setting = _context.Settings.Find(id);
            if (setting == null)
            {
                TempData["ErrorMessage"] = "Setting not found!";
                return RedirectToAction(nameof(Index));
            }

            _context.Settings.Remove(setting);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Setting deleted successfully!";
            return RedirectToAction(nameof(Index));
        }


        // Kiểm tra nếu Setting tồn tại trong DB
        private bool SettingExists(int id)
        {
            return _context.Settings.Any(e => e.Id == id);
        }
    }
}