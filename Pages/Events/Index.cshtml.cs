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

        public IList<Competitor> initial = new List<Competitor>();

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

            return RedirectToPage("./Results");
        }


    }
}
