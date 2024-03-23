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
    public class PlayerMatchesController : ControllerBase
    {
        private readonly ValorantContext _context;

        public PlayerMatchesController(ValorantContext context)
        {
            _context = context;
        }

        // GET: api/PlayerMatches/5
        [HttpGet("{matchId}")]
        public async Task<ActionResult<IEnumerable<PlayerMatch>>> GetPlayerMatchesAsync(int matchId)
        {
          if (_context.PlayerMatches == null)
          {
              return NotFound();
          }
            return await _context.PlayerMatches.Include("Player").Include("Player.Rank").Include("Agent").ToListAsync();
        }

        // GET: api/PlayerMatches/5
        [HttpGet("{matchId}/{playerId}")]
        public async Task<ActionResult<PlayerMatch>> GetPlayerMatchAsync(int matchId, int playerId)
        {
          if (_context.PlayerMatches == null)
          {
              return NotFound();
          }
            var playerMatch = await _context.PlayerMatches.Include("Player").Include("Player.Rank").Include("Agent").FirstOrDefaultAsync(it => it.MatchId == matchId && it.PlayerId == playerId);

            if (playerMatch == null)
            {
                return NotFound();
            }

            return playerMatch;
        }   
    }
}
