using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using WinnerPOV_API.Database;

namespace WinnerPOV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeasonController : ControllerBase
    {
        private readonly ValorantContext _context;

        public SeasonController(ValorantContext context)
        {
            _context = context;
        }


        [HttpGet("matches")]
        public async Task<ActionResult<IEnumerable<Match>>> GetSeasonalMatchesAsync()
        {
            Season? currentSeason = await _context.Seasons.FirstOrDefaultAsync(it => it.StartDate < DateTime.Now && it.EndDate > DateTime.Now); 

            if(currentSeason == null) {
                return NotFound();
             }

            return  await _context.Matches.Where(it => it.Date > currentSeason.StartDate && it.Date < currentSeason.EndDate).ToListAsync();
        }
    }
}
