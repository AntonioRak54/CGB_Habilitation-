using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGB_Habilitation.Controllers.Agent
{
    [Area("Agent")]
public class ProfilController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
}