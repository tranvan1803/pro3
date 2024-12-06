using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShowroomManagement.Data;
using ShowroomManagement.Models;

namespace ShowroomManagement.Controllers
{
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
            ViewBag.Vehicles = _context.Vehicles.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.Name
            }).ToList();

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

            ViewBag.Vehicles = _context.Vehicles.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.Name
            }).ToList();

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
    }
}
