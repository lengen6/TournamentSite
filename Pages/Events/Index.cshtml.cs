using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TieRenTournament.Models;
using TieRenTournament.Utils;
using Newtonsoft.Json;
using TieRenTournament.Data;
using static System.Net.Mime.MediaTypeNames;

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
        public List<Competitor> Initial { get; set; }
        public List<Competitor> PreviousParticipants { get; set; }

        public List<Competitor> Byes { get; set; }
        public List<Competitor> temp = new List<Competitor>();
        public Competitor compRed;
        public Competitor compBlue;
        public int round = 1;
        public int match = 1;

        public async Task<IActionResult> OnGetAsync()
        {
            
               if(_context.Competitor != null)
            {
                Competitors = _context.Competitor.ToList();
                Winners = await _context.Competitor.Where(w => w.Bracket == "Winner").ToListAsync();
                Losers = await _context.Competitor.Where(l => l.Bracket == "Loser").ToListAsync();
                Eliminated = await _context.Competitor.Where(e => e.Bracket == "Eliminated").ToListAsync();
                PreviousParticipants = await _context.Competitor.Where(b => b.PreviousParticipant == true).ToListAsync();
            }
            

            if (Losers.Count > 0)
            {
                DetermineByes(Losers);
                if (!IsBracketFinished(Losers))
                {
                    ArrangeMatch(Losers);
                }
            }

            if (Winners.Count > 0)
            {
                DetermineByes(Winners);
                bool isFinished = IsBracketFinished(Winners);
                if (!isFinished)
                {
                    ArrangeMatch(Winners);
                    ConductMatch(compRed, compBlue);
                    return RedirectToPage("./Match");
                }
            }

            bool RoundOver = IsRoundOver();

            if(RoundOver)
            {
                ResetParticipant();
                AlignLocalStateToDB(Winners, Losers, Eliminated);
                return RedirectToPage("./Index");
            }

            if(Eliminated.Count == Competitors.Count - 1)
            {
                AlignLocalStateToDB(Winners, Losers, Eliminated);
                return RedirectToPage("./Results");
            }
            
            return Redirect("~/");
        }

        //Class Methods go here

        public bool IsRoundOver()
        {
            bool isRoundOver = true;

            foreach (Competitor comp in Competitors)
            {
                
                if (comp.PreviousParticipant == false)
                {
                    isRoundOver = false;
                }
            }

            return isRoundOver;
        }
        public void ResetParticipant()
        {
            foreach (Competitor comp in Winners)
            {
                comp.PreviousParticipant = false;
        
            }

            foreach (Competitor comp in Losers)
            {
                comp.PreviousParticipant = false;
            }
        }

        //Method to reset lastMatch status

        public void ResetLastMatch(List<Competitor> argBracket)
        {
            foreach (Competitor remainingComp in argBracket)
            {
                remainingComp.LastMatch = false;
            }

        }

        public bool IsBracketFinished(List<Competitor> bracket)
        {
            bool isFinished = true;

            foreach(Competitor comp in bracket)
            {
                if(comp.PreviousParticipant == false)
                {
                    isFinished = false;
                }
            }

            return isFinished;
        }
        public void DetermineByes(List<Competitor> bracket)
        {
           bool wasPreviouslyRun = false;
            foreach (Competitor comp in bracket)
            {
                if (comp.PreviousParticipant)
                {
                    wasPreviouslyRun = true;
                }
            }

            if (!wasPreviouslyRun)
            {
               if(bracket.Count % 2 != 0)
                {
                    Random bracketPicker = new Random();
                    int currentPick = bracketPicker.Next(Losers.Count);
                    bracket[currentPick].Byes++;
                    bracket[currentPick].PreviousParticipant = true;
                }

            }

        }

         public Competitor PickPlayerAndRemoveFromBracket(List<Competitor> argBracket)
        {
            Random bracketPicker = new Random();
            int compPick = bracketPicker.Next(argBracket.Count);
            Competitor comp = argBracket[compPick];
            argBracket.RemoveAt(compPick);
            return comp;
        }

        public void ArrangeMatch(List<Competitor> argBracket)
        {
           
            
        }

        public IActionResult Match(Competitor argcompRed, Competitor argcompBlue)
        {
            //Flip a coin and determine Winners based on it. 0 is a win 1 is a loss

            Random coinFlip = new Random();

            //Set previous partipant and last match values

            argcompRed.PreviousParticipant = true;
            argcompRed.LastMatch = true;
            argcompBlue.PreviousParticipant = true;
            argcompBlue.LastMatch = true;

            if (coinFlip.Next(2) == 0)
            {
                argcompRed.Wins++;
                argcompBlue.Losses++;

                Winners.Add(argcompRed);

                if (argcompBlue.Losses > 1)
                {
                    Eliminated.Add(argcompBlue);
                }
                else
                {
                    Losers.Add(argcompBlue);
                }
            }
            else
            {
                argcompBlue.Wins++;
                argcompRed.Losses++;

                Winners.Add(argcompBlue);

                if (argcompRed.Losses > 1)
                {
                    Eliminated.Add(argcompRed);
                }
                else
                {
                    Losers.Add(argcompRed);
                }
            }

            AlignLocalStateToDB(Winners, Losers, Eliminated);
            return RedirectToPage("./Index");
        }

        public void AlignLocalStateToDB(List<Competitor> winnersToSave, List<Competitor> losersToSave, List<Competitor> eliminatedToSave)
        {

            if (winnersToSave.Count > 0)
            {
                foreach (var winner in winnersToSave)
                {
                    winner.Bracket = "Winner";
                    _context.Competitor.Attach(winner);
                }
            }

            if (losersToSave.Count > 0)
            {
                foreach (var loser in losersToSave)
                {
                    loser.Bracket = "Loser";
                    _context.Competitor.Attach(loser);
                }
            }

            if (eliminatedToSave.Count > 0)
            {
                foreach (var eliminated in eliminatedToSave)
                {
                    eliminated.Bracket = "Eliminated";
                    _context.Competitor.Attach(eliminated);
                }
            }

            _context.SaveChanges();
        }

        public void ConductMatch(Competitor compRed, Competitor compBlue)
        {
            //Asign Match participants by changing model field

            compRed.IsRedComp = true;
            compBlue.IsBlueComp = true;

            AlignLocalStateToDB(Winners, Losers, Eliminated);
        }
    }
}
