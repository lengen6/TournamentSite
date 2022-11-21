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

        MatchMakingHelper helper = new MatchMakingHelper();

        public List<Competitor> Initial { get; set; }

        public List<Competitor> Results { get; set; }

        public async Task<IActionResult> OnGetAsync()

        {
           
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(_context.Competitor != null)
            {
                Initial = await _context.Competitor.ToListAsync();
            }

            if(Initial.Count < 2)
            {
               return RedirectToPage("/Competitors/Index");
            }

            Results = helper.StartMatchMaking(Initial);

           
            TempData["compList"] = JsonConvert.SerializeObject(Results);
            

            return RedirectToPage("./Results");
        }


    }
}
