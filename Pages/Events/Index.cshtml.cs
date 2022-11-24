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
        public List<Competitor> Byes { get; set; }
        public List<Competitor> PreviousParticipants { get; set; }
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
                Byes = await _context.Competitor.Where(b => b.Bracket == "Bye").ToListAsync();
            }

            if (Eliminated.Count == Competitors.Count - 1)
            {
                Winners[0].Place = 1;
                List<Competitor> orderedByWins = Eliminated.OrderByDescending(e => e.Wins).ToList();
                for (int i = 0; i < orderedByWins.Count; i++)
                {
                    orderedByWins[i].Place = (i + 2);
                }
                AlignLocalStateToDB(Winners, Losers, Eliminated, Byes);
                return RedirectToPage("./Results");
            }

            if (Winners.Count == 1 && Losers.Count == 1)
            {
                compRed = Winners[0];
                compBlue = Losers[0];
                compRed.IsRedComp = true;
                compBlue.IsBlueComp = true;
                match++;
                AlignLocalStateToDB(Winners, Losers, Eliminated, Byes);
                return RedirectToPage("./Match");
            }

            if (Winners != null)
            {
                DetermineByes(Winners);
                if (!IsBracketFinished(Winners))
                {
                    if (ArrangeMatch(Winners))
                    {
                        match++;
                        AlignLocalStateToDB(Winners, Losers, Eliminated,Byes);
                        return RedirectToPage("./Match");
                    }
                    
                }
            }

            if (Losers != null)
            {
                DetermineByes(Losers);
                if (!IsBracketFinished(Losers))
                {
                    if (ArrangeMatch(Losers))
                    {
                        match++;
                        AlignLocalStateToDB(Winners, Losers, Eliminated, Byes);
                        return RedirectToPage("./Match");
                    }

                }
            }

            AlignLocalStateToDB(Winners, Losers, Eliminated, Byes);
            if(_context.Competitor != null)
            {
                Byes = await _context.Competitor.Where(b => b.Bracket == "Bye").ToListAsync();
            }

            if (Byes.Count > 1)
            {
                foreach (Competitor comp in Byes)
                {
                    comp.Byes--;
                    comp.PreviousParticipant = false;
                }

                if (ArrangeMatch(Byes))
                {
                    match++;
                    AlignLocalStateToDB(Winners, Losers, Eliminated, Byes);
                    return RedirectToPage("./Match");
                }
            }          

            bool RoundOver = IsRoundOver();

            if(RoundOver)
            {
                round++;
                ResetParticipant();
                AlignLocalStateToDB(Winners, Losers, Eliminated, Byes);
                return RedirectToPage("./Index");
            }
            
            return Redirect("~/");
        }

        //Class Methods go here

        public bool IsRoundOver()
        {
            bool isRoundOver = true;

            foreach (Competitor comp in Winners)
            {
                
                if (comp.PreviousParticipant == false)
                {
                    isRoundOver = false;
                }
            }

            foreach (Competitor comp in Losers)
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
                    int currentPick = bracketPicker.Next(bracket.Count);
                    bracket[currentPick].Byes++;
                    bracket[currentPick].Bracket = "Bye";
                    bracket[currentPick].PreviousParticipant = true;
                }

            }

        }

         public Competitor PickPlayer(List<Competitor> argBracket)
        {
            Random bracketPicker = new Random();
            int compPick = bracketPicker.Next(argBracket.Count);
            Competitor comp = argBracket[compPick];
            argBracket.RemoveAt(compPick);
            return comp;
        }

        public bool ArrangeMatch(List<Competitor> argBracket)
        {
            List<Competitor> viableCompetitors = new List<Competitor>();
            foreach(Competitor comp in argBracket)
            {
                if(comp.PreviousParticipant == false)
                {
                    viableCompetitors.Add(comp);
                }
            }

            if(viableCompetitors.Count > 1)
            {
                compRed = PickPlayer(viableCompetitors);
                compBlue = PickPlayer(viableCompetitors);
                compRed.IsRedComp = true;
                compBlue.IsBlueComp = true;
                return true;
            } else if (viableCompetitors[0] != null)
            {
                viableCompetitors[0].Byes++;
                viableCompetitors[0].Bracket = "Bye";
                viableCompetitors[0].PreviousParticipant = true;
                return false;
            }

            return true;
        }

        public void AlignLocalStateToDB(List<Competitor> winnersToSave, List<Competitor> losersToSave, List<Competitor> eliminatedToSave, List<Competitor> byesToSave)
        {

            if (winnersToSave.Count != null)
            {
                foreach (var winner in winnersToSave)
                {
                    winner.Bracket = "Winner";
                    _context.Competitor.Attach(winner);
                }
            }

            if (losersToSave.Count != null)
            {
                foreach (var loser in losersToSave)
                {
                    loser.Bracket = "Loser";
                    _context.Competitor.Attach(loser);
                }
            }

            if (eliminatedToSave != null)
            {
                foreach (var eliminated in eliminatedToSave)
                {
                    eliminated.Bracket = "Eliminated";
                    _context.Competitor.Attach(eliminated);
                }
            }

            if (byesToSave != null)
            {
                foreach (var bye in byesToSave)
                {
                    bye.Bracket = "Bye";
                    _context.Competitor.Attach(bye);
                }
            }

            _context.SaveChanges();
        }

    }
}
