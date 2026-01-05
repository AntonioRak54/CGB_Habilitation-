using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CGB_Habilitation.Data;
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
                return View("~/Views/Auth/Login.cshtml");
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

        public IActionResult EnAttente() => View("~/Views/Auth/EnAttente.cshtml");

        public IActionResult DemandeCreation() => View("~/Views/Auth/DemandeCreation.cshtml");

        [HttpPost]
        public IActionResult DemandeCreation(CGB_Habilitation.Models.Agent agent)
        {
            agent.EstValide = false;
            _context.Agents.Add(agent);
            _context.SaveChanges();
            return RedirectToAction("EnAttente");
            //Placed on the page EnAttente.cshtml
        }

        [HttpGet]
        public IActionResult Attente() => View("~/Views/Auth/Attente.cshtml");

        
    }

}