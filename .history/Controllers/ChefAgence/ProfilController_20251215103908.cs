using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CGB_Habilitation.Data;
using Microsoft.AspNetCore.Mvc;

namespace CGB_Habilitation.Controllers.ChefAgence
{
    [Area("ChefAgence")]
public class ProfilController : Controller
{
    private readonly HabilitationDbContext _context;

    public ProfilController(HabilitationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View(); // données simplifiées
    }
}
}