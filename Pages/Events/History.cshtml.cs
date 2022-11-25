using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TieRenTournament.Models;

namespace TieRenTournament.Pages.Events
{
    public class HistoryModel : PageModel
    {
        private readonly TieRenTournament.Data.ApplicationDbContext _context;

        public HistoryModel(TieRenTournament.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Match> Matches { get; set; }
        public void OnGet()
        {
            Matches = _context.Match.ToList();
        }
    }
}
