using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TieRenTournament.Models;
using TieRenTournament.Utils;
using Newtonsoft.Json;

namespace TieRenTournament.Pages.Events
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private static TieRenTournament.Data.ApplicationDbContext _context;

        public IndexModel(TieRenTournament.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Initial> Initial { get; set; }
        public List<Winner> Winners { get; set; }

        public List<Loser> Losers { get; set; }
        public List<Eliminated> Eliminated { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {

            Initial = await _context.Competitor.OfType<Initial>().ToListAsync();

            if (_context.Winner != null)
            {
                Winners = await _context.Competitor.OfType<Winner>().ToListAsync();
            }

            if (_context.Loser != null)
            {
                Losers = await _context.Competitor.OfType<Loser>().ToListAsync();
            }

            if (_context.Eliminated != null)
            {
                Eliminated = await _context.Competitor.OfType<Eliminated>().ToListAsync();
            }

            if (Initial.Count < 2)
            {
               return RedirectToPage("/Competitors/Index");
            }

            MatchMakingHelper helper = new MatchMakingHelper(Initial,Winners, Losers, Eliminated);

            if(Winners == null && Losers == null)
            {
                List<Competitor> competitors = await _context.Competitor.ToListAsync();
                helper.StartMatchmaking(competitors);
            }
            else
            {

            }

           
            //TempData["compList"] = JsonConvert.SerializeObject(Results);
            

            return RedirectToPage("./Results");
        }


    }
}
