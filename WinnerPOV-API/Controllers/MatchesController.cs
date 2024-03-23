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
    public class MatchesController : ControllerBase
    {
        private readonly ValorantContext _context;

        public MatchesController(ValorantContext context)
        {
            _context = context;
        }

        // GET: api/Matches/seasonal
        [HttpGet("season/{seasonName}")]
        public async Task<ActionResult<IEnumerable<Match>>> GetMatchesAsync(string seasonName)
        {
            if (_context.Seasons == null)
            {
                return NotFound();
            }

            Season? season = await _context.Seasons.FirstOrDefaultAsync(it => it.Name.Equals(seasonName, StringComparison.OrdinalIgnoreCase));
            if (season == null)
            {
                return NotFound();
            }

            if (_context.Matches == null)
            {
                return NotFound();
            }

            return await _context.Matches.Include("Map").Include("PlayerMatches").Include("PlayerMatches.Player").Include("PlayerMatches.Agent").Include("PlayerMatches.Player.Rank").Where(it => it.Date > season.StartDate && it.Date < season.EndDate).ToListAsync();
        }

        // GET: api/Matches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Match>> GetMatchAsync(int id)
        {
            if (_context.Matches == null)
            {
                return NotFound();
            }
            var match = await _context.Matches.Include("Map").Include("PlayerMatches").Include("PlayerMatches.Player").Include("PlayerMatches.Agent").Include("PlayerMatches.Player.Rank").FirstOrDefaultAsync(it => it.MatchId == id);

            if (match == null)
            {
                return NotFound();
            }

            return match;
        }
    }
}
