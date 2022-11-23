using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using TieRenTournament.Models;

namespace TieRenTournament.Pages.Events
{
    public class MatchModel : PageModel
    {
        private readonly TieRenTournament.Data.ApplicationDbContext _context;

        public MatchModel(TieRenTournament.Data.ApplicationDbContext context)
        {
            _context = context;
        }
 
        public Competitor? RedComp { get; set; }
        public Competitor? BlueComp { get; set; }
        [BindProperty]
        public bool RedWins { get; set; }
        public void OnGet()
        {
            if(_context.Competitor != null)
            {
                RedComp = _context.Competitor.Where(r => r.IsRedComp == true).FirstOrDefault();
                BlueComp = _context.Competitor.Where(b => b.IsBlueComp == true).FirstOrDefault();

                if (RedComp != null && BlueComp != null)
                {
                    RedComp.PreviousParticipant = true;
                    RedComp.LastMatch = true;
                    BlueComp.PreviousParticipant = true;
                    BlueComp.LastMatch = true;

                    _context.Attach(RedComp);
                    _context.Attach(BlueComp);
                    _context.SaveChangesAsync();
                }
            }
        }

        public IActionResult OnPost()
        {

            if (_context.Competitor != null)
            {
                RedComp = _context.Competitor.Where(r => r.IsRedComp == true).FirstOrDefault();
                BlueComp = _context.Competitor.Where(b => b.IsBlueComp == true).FirstOrDefault();
            }

                if (RedComp != null && BlueComp != null)
            {
                if (RedWins)
                {
                    RedComp.Wins++;
                    BlueComp.Losses++;

                    RedComp.Bracket = "Winner";

                    if (BlueComp.Losses > 1)
                    {
                        BlueComp.Bracket = "Eliminated";
                    }
                    else
                    {
                        BlueComp.Bracket = "Loser";
                    }
                } else if (!RedWins)
                {
                    BlueComp.Wins++;
                    RedComp.Losses++;

                    BlueComp.Bracket = "Winner";

                    if (RedComp.Losses > 1)
                    {
                        RedComp.Bracket = "Eliminated";
                    }
                    else
                    {
                        RedComp.Bracket = "Loser";
                    }
                }
                RedComp.IsRedComp = false;
                BlueComp.IsBlueComp = false;
                _context.Competitor.Attach(RedComp);
                _context.Competitor.Attach(BlueComp);
                _context.SaveChanges();
            }

            return RedirectToPage("./Index");
        }
    }
}
