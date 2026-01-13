using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CGB_Habilitation.Data;
using CGB_Habilitation.Models;
using Microsoft.AspNetCore.Mvc;

namespace CGB_Habilitation.Controllers.Admin
{
//    [Area("Admin")]
public class ParametreController : Controller
{
    private readonly HabilitationDbContext _context;

    public ParametreController(HabilitationDbContext context)
    {
        _context = context;
    }

    // // ROLES
    // public IActionResult Roles() => View(_context.Roles.ToList());  

    public IActionResult Roles()
    {
        var roles = _context.Roles.ToList();
        return View("~/Views/Admin/Parametre/Roles.cshtml", roles);
    }

    [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                role.IdRole = Guid.NewGuid();
                _context.Roles.Add(role);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(role);
        }

    // AGENCES
    public IActionResult Agences() => View(_context.Agences.ToList());

    public IActionResult CreateAgence()
    {
        var agences = _context.Agences.ToList();
        return View("~/Views/Admin/Parametre/Agences.cshtml", agences);
    }

    [HttpPost]
    public IActionResult CreateAgence(Agence agence)
    {
        _context.Agences.Add(agence);
        _context.SaveChanges();
        return RedirectToAction("Agences");
    }

    // SERVICES
    public IActionResult Services() => View(_context.Services.ToList());

    public IActionResult CreateService()
    {
        var services = _context.Services.ToList();
        return View("~/Views/Admin/Parametre/Services.cshtml", services);
    }

    [HttpPost]
    public IActionResult CreateService(Service service)
    {
        _context.Services.Add(service);
        _context.SaveChanges();
        return RedirectToAction("Services");
    }
}
}