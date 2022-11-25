using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TieRenTournament.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace TieRenTournament.Pages.Events
{
    public class ResultsModel : PageModel
    {
        private static TieRenTournament.Data.ApplicationDbContext _context;

        public ResultsModel(TieRenTournament.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Competitor> Results { get; set; }
        public List<Match> Matches { get; set; }
        public void OnGetAsync()
        {
            
            Results = _context.Competitor.OrderBy(c => c.Place).ToList();

            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            Results = await _context.Competitor.ToListAsync();
            Matches = await _context.Match.ToListAsync();

            foreach(Match match in Matches)
            {
                _context.Match.Remove(match);
            }

            foreach (var competitor in Results)
            {
                competitor.Bracket = "Winner";
                competitor.Place = 0;
                competitor.PreviousParticipant = false;
                competitor.LastMatch = false;
                competitor.Byes = 0;
                competitor.Wins = 0;
                competitor.Losses = 0;
                competitor.IsRedComp = false;
                competitor.IsBlueComp = false;
                _context.Competitor.Attach(competitor);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/Competitors/Index");
        } 
    }
}
