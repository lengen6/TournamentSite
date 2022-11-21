using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TieRenTournament.Models;

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

        //Declare the initial, winners, losers, and eliminated brackets as lists of competitors

        public IList<Competitor> initial = new List<Competitor>();
        public IList<Competitor> winners = new List<Competitor>();
        public IList<Competitor> losers = new List<Competitor>();
        public IList<Competitor> eliminated = new List<Competitor>();
        public IList<Competitor> temp = new List<Competitor>();


        //Declare global variables

        Random bracketPicker = new Random();

        public static Competitor compRed;
        public static Competitor compBlue;
        public static int round = 1;
        public static int match = 1;
        public async Task<IActionResult> OnGetAsync()
        {
            if(_context.Competitor != null)
            {
                initial = await _context.Competitor.ToListAsync();
            }

            if(initial.Count < 2)
            {
               return RedirectToPage("/Competitors/Index");
            }

            return Page();
        }
    }
}
