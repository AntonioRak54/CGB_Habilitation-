using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CGB_Habilitation.Data;
using Microsoft.AspNetCore.Mvc;

namespace CGB_Habilitation.Controllers
{
    public class AuthController : Controller
    {
        private readonly HabilitationDbContext _context;

        public AuthController(HabilitationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            var agent = _context.Agents
                .Include(a => a.RoleAgent)
                .FirstOrDefault(a => a.LoginAgent == login && a.PasswordAgent == password);

            if (agent == null)
            {
                ViewBag.Error = "Login ou mot de passe incorrect";
                return View();
            }

            if (!agent.EstValide)
                return RedirectToAction("EnAttente");

            agent.DateLogin = DateTime.Now;
            _context.SaveChanges();

            return agent.RoleAgent.NomRole switch
            {
                "Admin" => RedirectToAction("Index", "Agent", new { area = "Admin" }),
                "ChefAgence" => RedirectToAction("Index", "Habilitation", new { area = "ChefAgence" }),
                _ => RedirectToAction("Index", "Profil", new { area = "Agent" })
            };
        }

        public IActionResult EnAttente() => View();

        public IActionResult DemandeCreation() => View();

        [HttpPost]
        public IActionResult DemandeCreation(Agent agent)
        {
            agent.EstValide = false;
            _context.Agents.Add(agent);
            _context.SaveChanges();
            return RedirectToAction("EnAttente");
        }
    }

}