using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CGB_Habilitation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CGB_Habilitation.Controllers.Admin
{
    // [Area("Admin")]
public class AgentController : Controller
{
    private readonly HabilitationDbContext _context;

    public AgentController(HabilitationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
        => View(_context.Agents.Include(a => a.RoleAgent).ToList());

    public IActionResult Valider(Guid id)
    {
        var agent = _context.Agents.Find(id);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}

}