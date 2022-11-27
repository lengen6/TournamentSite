using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
        [FromQuery(Name = "elimination")]
        public int Elimination { get; set; }
        [FromQuery(Name = "match")]
        public int Match { get; set; }
        [FromQuery(Name = "round")]
        public int Round { get; set; } = 1;
        public Competitor? RedComp { get; set; }
        public Competitor? BlueComp { get; set; }
        [BindProperty]
        public bool RedWins { get; set; }
        [BindProperty]
        public int RedScore { get; set; }
        [BindProperty]
        public int BlueScore { get; set; }
        [BindProperty]
        public string VictoryMethod { get; set; }
        [BindProperty]
        public int StartMinutes { get; set; }
        [BindProperty]
        public int StartSeconds { get; set; }
        [BindProperty]
        public int EndMinutes { get; set; }
        [BindProperty]
        public int EndSeconds { get; set; }


        public void OnGet()
        {
            if (_context.Competitor != null)
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

        public IActionResult OnPost(int elimination, int match, int round)
        {
            if (_context.Competitor != null)
            {
                RedComp = _context.Competitor.Where(r => r.IsRedComp == true).FirstOrDefault();
                BlueComp = _context.Competitor.Where(b => b.IsBlueComp == true).FirstOrDefault();
                SetWinner(RedComp, BlueComp, elimination, match, round);
                CreateMatch(RedComp, BlueComp, elimination, match, round);
                _context.SaveChanges();
            }

            return RedirectToPage("./Index", new {elimination = elimination, match = match, round = round});
        }

        //Class Methods
        public void CreateMatch(Competitor redComp, Competitor blueComp, int elimination, int match, int round)
        {
            string startTime = StartMinutes.ToString() + " : " + StartSeconds.ToString();
            string endTime = EndMinutes.ToString() + " : " + EndSeconds.ToString();
            string duration = (StartMinutes - EndMinutes).ToString() + " : " + (StartSeconds - EndSeconds).ToString();

            Match currentMatch = new Match();
            currentMatch.CompeitorRed = redComp;
            currentMatch.CompeitorBlue = blueComp;
            currentMatch.MatchNumber = match;
            currentMatch.RoundNumber = round;
            currentMatch.Bracket = redComp.Bracket;
            currentMatch.VictoryMethod = VictoryMethod;
            currentMatch.CompetitorRedScore = RedScore;
            currentMatch.CompetitorBlueScore = BlueScore;
            currentMatch.StartingLength = startTime;
            currentMatch.MatchEnd = endTime;
            currentMatch.Duration = duration;

            if (RedWins)
            {
                currentMatch.Winner = redComp;
            }
            else
            {
                currentMatch.Winner = blueComp;
            }

            _context.Match.Add(currentMatch);
        }
        public void SetWinner(Competitor redComp, Competitor blueComp, int elimination, int match, int round)
        {
            if (RedComp != null && BlueComp != null)
            {
                if (RedWins)
                {
                    RedComp.Wins++;
                    BlueComp.Losses++;

                    RedComp.Bracket = "Winner";

                    if (BlueComp.Losses >= elimination)
                    {
                        BlueComp.Bracket = "Eliminated";
                    }
                    else
                    {
                        BlueComp.Bracket = "Loser";
                    }
                }
                else if (!RedWins)
                {
                    BlueComp.Wins++;
                    RedComp.Losses++;

                    BlueComp.Bracket = "Winner";

                    if (RedComp.Losses >= elimination)
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
            }
        }
    }
}
