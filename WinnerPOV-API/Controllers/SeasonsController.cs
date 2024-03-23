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
    public class SeasonsController : ControllerBase
    {
        private readonly ValorantContext _context;

        public SeasonsController(ValorantContext context)
        {
            _context = context;
        }

        // GET: api/Seasons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Season>>> GetSeasonsAsync()
        {
          if (_context.Seasons == null)
          {
              return NotFound();
          }
            return await _context.Seasons.ToListAsync();
        }

        // GET: api/Seasons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Season>> GetSeasonAsync(int id)
        {
          if (_context.Seasons == null)
          {
              return NotFound();
          }
            var season = await _context.Seasons.FindAsync(id);

            if (season == null)
            {
                return NotFound();
            }

            return season;
        }
    }
}
