using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CGB_Habilitation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestDbController : ControllerBase
    {
        private readonly HabilitationDbContext _context;

        public TestDbController(HabilitationDbContext context)
        {
            _context = context;
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetDatabaseStatus()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                
                var roles = await _context.Roles.ToListAsync();
                
                var tables = new
                {
                    DatabaseConnection = canConnect ? "OK" : "FAILED",
                    Tables = new
                    {
                        Agents = await _context.Agents.CountAsync(),
                        Agences = await _context.Agences.CountAsync(),
                        Services = await _context.Services.CountAsync(),
                        Roles = roles.Count,
                        Habilitations = await _context.Habilitations.CountAsync()
                    },
                    Roles = roles.Select(r => r.NomRole)
                };
                
                return Ok(tables);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}