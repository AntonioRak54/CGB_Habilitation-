using HabilitationApp.Models;
using HabilitationApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HabilitationApp.Controllers
{
    public class UtilisateurController : Controller
    {
        private readonly IUtilisateurRepository _repo;

        public UtilisateurController(IUtilisateurRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var utilisateurs = await _repo.GetWithRolesAsync();
            return View(utilisateurs);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        public IActionResult Create() => View(new Utilisateur());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Utilisateur user)
        {
            if (!ModelState.IsValid) return View(user);
            user.UserId = Guid.NewGuid();
            user.DateCreation = DateTime.UtcNow;
            await _repo.AddAsync(user);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Utilisateur user)
        {
            if (id != user.UserId) return BadRequest();
            if (!ModelState.IsValid) return View(user);
            await _repo.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        
    }
}
