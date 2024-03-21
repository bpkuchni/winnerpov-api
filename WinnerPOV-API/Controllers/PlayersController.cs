using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WinnerPOV_API.Database;

namespace WinnerPOV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly ValorantContext _context;

        public PlayersController(ValorantContext context)
        {
            _context = context;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
          if (_context.Players == null)
          {
              return NotFound();
          }
            return await _context.Players.ToListAsync();
        }

        // GET: api/Players/broondoom/na1
        [HttpGet("{name}/{tag}")]
        public async Task<ActionResult<Player>> GetPlayer(string name, string tag)
        {
          if (_context.Players == null)
          {
              return NotFound();
          }
            Player? player = await _context.Players.FirstOrDefaultAsync(it => it.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && it.Tag.Equals(tag, StringComparison.OrdinalIgnoreCase));

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }
    }
}
