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
        public void OnGetAsync()
        {
            
            Results = _context.Competitor.ToList();
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            Results = await _context.Competitor.ToListAsync();
            

            foreach (var competitor in Results)
            {
                competitor.Bracket = "Winner";
                competitor.Place = 0;
                competitor.PreviousParticipant = false;
                competitor.LastMatch = false;
                competitor.Byes = 0;
                competitor.Wins = 0;
                competitor.Losses = 0;
                _context.Competitor.Attach(competitor);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/Competitors/Index");
        } 
    }
}
