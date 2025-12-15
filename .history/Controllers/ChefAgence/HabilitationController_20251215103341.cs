using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CGB_Habilitation.Data;
using CGB_Habilitation.Models;
using Microsoft.AspNetCore.Mvc;

namespace CGB_Habilitation.Controllers.Agent
{
    [Area("ChefAgence")]
public class HabilitationController : Controller
{
    private readonly HabilitationDbContext _context;

    public HabilitationController(HabilitationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
        => View(_context.Habilitations.ToList());

    public IActionResult Create()
        => View();

    [HttpPost]
    public IActionResult Create(Habilitation h)
    {
        h.Etat = "En attente";
        h.DateCreationDemande = DateTime.Now;
        _context.Habilitations.Add(h);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}

}