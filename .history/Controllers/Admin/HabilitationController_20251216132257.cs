using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CGB_Habilitation.Data;
using CGB_Habilitation.Models;
using Microsoft.AspNetCore.Mvc;

namespace CGB_Habilitation.Controllers.Admin
{
    public class HabilitationController : Controller    {
        private readonly HabilitationDbContext _context;

        public HabilitationController(HabilitationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            var agents = _context.Agents.ToList();
            ViewBag.Agents = agents;
            return View(agents);
        }

        [HttpPost]
        public IActionResult Create(Habilitation h)
        {
            return View();
        }

        public IActionResult Edit(Guid id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(Habilitation h)
        {
            return View();
        }
    }
}