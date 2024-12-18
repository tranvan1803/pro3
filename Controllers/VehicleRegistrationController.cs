using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShowroomManagement.Data;
using ShowroomManagement.Models;

namespace ShowroomManagement.Controllers
{
    [Authorize]
    public class VehicleRegistrationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehicleRegistrationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VehicleRegistration
        public async Task<IActionResult> Index()
        {
            var registrations = await _context.VehicleRegistrations.Include(v => v.Vehicle).ToListAsync();
            return View(registrations);
        }

        // GET: VehicleRegistration/Create
        public IActionResult Create()
        {
            PopulateVehiclesDropdown();
            return View();
        }

        // POST: VehicleRegistration/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleRegistration registration)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateVehiclesDropdown();
            return View(registration);
        }

        // GET: VehicleRegistration/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var registration = await _context.VehicleRegistrations
                .Include(v => v.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (registration == null)
                return NotFound();

            return View(registration);
        }

        // GET: VehicleRegistration/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleRegistration = await _context.VehicleRegistrations.FindAsync(id);
            if (vehicleRegistration == null)
            {
                return NotFound();
            }

            ViewBag.Vehicles = new SelectList(_context.Vehicles, "Id", "Name", vehicleRegistration.VehicleId);
            return View(vehicleRegistration);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleRegistration vehicleRegistration)
        {
            if (id != vehicleRegistration.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicleRegistration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.VehicleRegistrations.Any(e => e.Id == vehicleRegistration.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Vehicles = new SelectList(_context.Vehicles, "Id", "Name", vehicleRegistration.VehicleId);
            return View(vehicleRegistration);
        }

        // GET: VehicleRegistration/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var registration = await _context.VehicleRegistrations
                .Include(v => v.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (registration == null)
                return NotFound();

            return View(registration);
        }

        // POST: VehicleRegistration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registration = await _context.VehicleRegistrations.FindAsync(id);
            if (registration != null)
            {
                _context.VehicleRegistrations.Remove(registration);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper method to populate dropdown list
        private void PopulateVehiclesDropdown()
        {
            ViewBag.Vehicles = _context.Vehicles.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.Name
            }).ToList();
        }

        // Helper method to check if registration exists
        private bool VehicleRegistrationExists(int id)
        {
            return _context.VehicleRegistrations.Any(e => e.Id == id);
        }

    }
}