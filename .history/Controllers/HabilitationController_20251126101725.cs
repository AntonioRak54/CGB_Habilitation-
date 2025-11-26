using HabilitationApp.Models;
using HabilitationApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace HabilitationApp.Controllers
{
    public class HabilitationController : Controller
    {
        private readonly AppDbContext _ctx;
        public HabilitationController(AppDbContext ctx) { _ctx = ctx; }

        public async Task<IActionResult> Index()
        {
            var list = await _ctx.Habilitations.Include(h => h.Utilisateur).Include(h => h.Profil).ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var item = await _ctx.Habilitations.Include(h => h.Utilisateur).Include(h => h.Profil)
                .FirstOrDefaultAsync(h => h.HabilitationId == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public IActionResult Create() => View(new Habilitation());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Habilitation hab)
        {
            if (!ModelState.IsValid) return View(hab);
            hab.HabilitationId = Guid.NewGuid();
            hab.DateDemande = DateTime.UtcNow;
            await _ctx.Habilitations.AddAsync(hab);
            await _ctx.SaveChangesAsync();
            // TODO: envoyer notif
            return RedirectToAction(nameof(Index));
        }

        // Edit + Delete idem
    }
}
