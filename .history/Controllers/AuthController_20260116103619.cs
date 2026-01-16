using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CGB_Habilitation.Controllers
{
    public class AuthController : Controller
    {
        private readonly HabilitationDbContext _context;

        public AuthController(HabilitationDbContext context)
        {
            _context = context;
        }

        // GET: /Auth/Login
        public IActionResult Login() => View();

        // POST: /Auth/Login
        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            // Recherche de l'agent avec son rôle
            var agent = _context.Agents
                .Include(a => a.RoleAgent)
                .FirstOrDefault(a => a.LoginAgent == login && a.PasswordAgent == password);

            if (agent == null)
            {
                ViewBag.Error = "Login ou mot de passe incorrect";
                return View(); // Retourne à la vue Login
            }

            // Vérification si l'agent est validé
            if (!agent.EstValide)
            {
                return RedirectToAction("EnAttente");
            }

            // Mise à jour de la date de connexion
            agent.DateLogin = DateTime.Now;
            _context.SaveChanges();

            // Redirection directe selon le rôle
            // IMPORTANT: Utilisez exactement les mêmes noms que dans votre base de données
            return agent.RoleAgent.NomRole.Trim().ToLower() switch
            {
                "admin" => RedirectToAction("Index", "Dashboard", new { area = "Admin" }),
                "chefagence" => RedirectToAction("Index", "Dashboard", new { area = "ChefAgence" }),
                "agent" => RedirectToAction("Index", "Dashboard", new { area = "Agent" }),
                _ => RedirectToAction("AccesRefuse") // Rôle non reconnu
            };
        }

        // Action pour accès refusé
        public IActionResult AccesRefuse()
        {
            return View();
        }

        // Autres actions existantes...
        public IActionResult EnAttente() => View();

        public IActionResult DemandeCreation() => View();

        [HttpPost]
        public IActionResult DemandeCreation(CGB_Habilitation.Models.Agent agent)
        {
            agent.EstValide = false;
            _context.Agents.Add(agent);
            _context.SaveChanges();
            return RedirectToAction("EnAttente");
        }

        [HttpGet]
        public IActionResult Attente() => View();
    }
}