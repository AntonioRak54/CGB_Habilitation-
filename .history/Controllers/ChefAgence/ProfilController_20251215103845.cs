using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGB_Habilitation.Controllers.ChefAgence
{
    [Area("ChefAgence")]
public class ProfilController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProfilController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View(); // données simplifiées
    }
}
}