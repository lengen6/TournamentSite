using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TieRenTournament.Data;
using TieRenTournament.Models;

namespace TieRenTournament.Pages.Competitors
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly TieRenTournament.Data.ApplicationDbContext _context;

        public IndexModel(TieRenTournament.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Competitor> Competitor { get;set; } = default!;
        [BindProperty]
        public int Elimination { get; set; } = 2;

        public async Task OnGetAsync()
        {
            if (_context.Competitor != null)
            {
                Competitor = await _context.Competitor.ToListAsync();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return RedirectToPage("/Events/Index", new {elimination = Elimination});
        }
    }
}
