using System;
using System.Linq;
using CGB_Habilitation.Data;
using CGB_Habilitation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CGB_Habilitation.Controllers.Agent
{
    // [Area("ChefAgence")]
    public class HabilitationController : Controller
    {
        private readonly HabilitationDbContext _context;

        public HabilitationController(HabilitationDbContext context)
        {
            _context = context;
        }

        // GET: Liste des habilitations
        public IActionResult Index()
        {
            var habilitations = _context.Habilitations
                .Include(h => h.Agent)
                .ToList();
            return View("~/Views/ChefAgence/Habilitation/Index.cshtml", habilitations);
        }

        // GET: Formulaire de création
        public IActionResult Create()
        {
            ViewBag.Agents = _context.Agents.ToList();
            return View();
        }

        // POST: Créer une habilitation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Habilitation habilitation)
        {
            if (ModelState.IsValid)
            {
                habilitation.IdHabilitation = Guid.NewGuid();
                habilitation.Etat = "En attente";
                habilitation.DateCreationDemande = DateTime.Now;
                
                _context.Habilitations.Add(habilitation);
                _context.SaveChanges();
                
                TempData["SuccessMessage"] = "Habilitation créée avec succès!";
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Agents = _context.Agents.ToList();
            return View("~/Views/ChefAgence/Habilitation/Create.cshtml", habilitations);
        }

        // GET: Détails d'une habilitation
        public IActionResult Details(Guid id)
        {
            var habilitation = _context.Habilitations
                .Include(h => h.Agent)
                .FirstOrDefault(h => h.IdHabilitation == id);
                
            if (habilitation == null)
            {
                return NotFound();
            }
            
            return View(habilitation);
        }

        // GET: Formulaire d'édition
        public IActionResult Edit(Guid id)
        {
            var habilitation = _context.Habilitations.Find(id);
            if (habilitation == null)
            {
                return NotFound();
            }
            
            ViewBag.Agents = _context.Agents.ToList();
            return View(habilitation);
        }

        // POST: Mettre à jour une habilitation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Habilitation habilitation)
        {
            if (id != habilitation.IdHabilitation)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingHabilitation = _context.Habilitations.Find(id);
                    if (existingHabilitation == null)
                    {
                        return NotFound();
                    }

                    // Mettre à jour les propriétés
                    existingHabilitation.TypeHabilitation = habilitation.TypeHabilitation;
                    existingHabilitation.Etat = habilitation.Etat;
                    existingHabilitation.DateDebut = habilitation.DateDebut;
                    existingHabilitation.DateFin = habilitation.DateFin;
                    existingHabilitation.Motif = habilitation.Motif;
                    existingHabilitation.PieceJoint = habilitation.PieceJoint;
                    existingHabilitation.IdAgent = habilitation.IdAgent;

                    _context.Update(existingHabilitation);
                    _context.SaveChanges();
                    
                    TempData["SuccessMessage"] = "Habilitation mise à jour avec succès!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HabilitationExists(id))
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
            
            ViewBag.Agents = _context.Agents.ToList();
            return View(habilitation);
        }

        // GET: Confirmation de suppression
        public IActionResult Delete(Guid id)
        {
            var habilitation = _context.Habilitations
                .Include(h => h.Agent)
                .FirstOrDefault(h => h.IdHabilitation == id);
                
            if (habilitation == null)
            {
                return NotFound();
            }
            
            return View(habilitation);
        }

        // POST: Supprimer une habilitation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var habilitation = _context.Habilitations.Find(id);
            if (habilitation != null)
            {
                _context.Habilitations.Remove(habilitation);
                _context.SaveChanges();
                
                TempData["SuccessMessage"] = "Habilitation supprimée avec succès!";
            }
            
            return RedirectToAction(nameof(Index));
        }

        // Action pour traiter une habilitation (approuver/rejeter)
        public IActionResult Traiter(Guid id, string nouvelEtat, Guid? traiterPar = null)
        {
            var habilitation = _context.Habilitations.Find(id);
            if (habilitation == null)
            {
                return NotFound();
            }

            habilitation.Etat = nouvelEtat;
            habilitation.TraiterPar = traiterPar;
            
            if (nouvelEtat == "Approuvée")
            {
                habilitation.DateDebut = DateTime.Now;
            }
            
            _context.SaveChanges();
            
            TempData["SuccessMessage"] = $"Habilitation {nouvelEtat.ToLower()} avec succès!";
            return RedirectToAction(nameof(Index));
        }

        private bool HabilitationExists(Guid id)
        {
            return _context.Habilitations.Any(e => e.IdHabilitation == id);
        }
    }
}