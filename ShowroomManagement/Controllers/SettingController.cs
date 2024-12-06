using Microsoft.AspNetCore.Mvc;
using ShowroomManagement.Data;
using ShowroomManagement.Models;

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
}
