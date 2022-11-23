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
        private readonly TieRenTournament.Data.ApplicationDbContext _context;

        public IndexModel(TieRenTournament.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        List<Competitor> Competitors { get; set; }
        public List<Competitor> Winners { get; set; }

        public List<Competitor> Losers { get; set; }
        public List<Competitor> Eliminated { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            if(_context.Competitor != null)
            {
                Competitors = await _context.Competitor.ToListAsync();
                Winners = await _context.Competitor.Where(w => w.Bracket == "Winner").ToListAsync();
                Losers = await _context.Competitor.Where(l => l.Bracket == "Loser").ToListAsync();
                Eliminated = await _context.Competitor.Where(e => e.Bracket == "Eliminated").ToListAsync();
            }

            MatchMakingHelper helper = new MatchMakingHelper(Winners, Losers, Eliminated, _context);

          
                if(helper.MatchMaking(Competitors) == true)
                {
                    return RedirectToPage("./Results");
                }

            return RedirectToPage("./Index");

            //TempData["compList"] = JsonConvert.SerializeObject(Results);

        }


    }
}
