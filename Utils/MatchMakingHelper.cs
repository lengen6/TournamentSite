using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TieRenTournament.Data;
using TieRenTournament.Models;

namespace TieRenTournament.Utils
{
    public class MatchMakingHelper
    {
        ApplicationDbContext Context { get; set; }
        public List<Competitor> Winners { get; set; }
        public List<Competitor> Losers { get; set; }
        public List<Competitor> Eliminated { get; set; }
        public List<Competitor> temp = new List<Competitor>();
        public List<Competitor> Next = new List<Competitor>();

        public MatchMakingHelper(List<Competitor> winners, List<Competitor> losers, List<Competitor> eliminated, ApplicationDbContext context)
        {
            Context = context;
            Winners = winners;
            Losers = losers;
            Eliminated = eliminated;
        }

        public Competitor compRed;
        public Competitor compBlue;
        public int round = 1;
        public int match = 1;

        Random bracketPicker = new Random();

        //Method for resetting previous participant after each round

        public void AlignLocalStateToDB(List<Competitor> winnersToSave, List<Competitor> losersToSave, List<Competitor> eliminatedToSave)
        {

          if(winnersToSave.Count > 0)
            {
                foreach (var winner in winnersToSave)
                {
                    winner.Bracket = "Winner";
                    Context.Competitor.Attach(winner);
                }
            }

           if(losersToSave.Count > 0)
            {
                foreach (var loser in losersToSave)
                {
                    loser.Bracket = "Loser";
                    Context.Competitor.Attach(loser);
                }
            }

           if(eliminatedToSave.Count > 0)
            {
                foreach (var eliminated in eliminatedToSave)
                {
                    eliminated.Bracket = "Eliminated";
                    Context.Competitor.Attach(eliminated);
                }
            }

            Context.SaveChanges();
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

        //Method to randomly pick a player from a bracket

        public Competitor PickPlayer(List<Competitor> argBracket)
        {
            int compPick = bracketPicker.Next(argBracket.Count);
            Competitor comp = argBracket[compPick];
            argBracket.RemoveAt(compPick);
            return comp;
        }

        //Method for determining Winners and Losers before integration with the scorekeeping app
        //I kept this method in case testing ever needs to be done on the bracket portion of the app in the future
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
            return new RedirectResult("/Events/Index");
        }

        //Method for running through a bracket

        public ActionResult RunBracket(List<Competitor> argBracket)
        {
            int numNonPreviousParts = 0;

            foreach (Competitor comp in argBracket)
            {
                if (comp.PreviousParticipant == false)
                {
                    numNonPreviousParts++;
                }
            }

            //Logic for rounds 2 and on

            bool compRedCheck = false;
            bool compBlueCheck = false;

            //Conduct all the matches in a bracket

            //Loop through bracket and determine Winners
            if (numNonPreviousParts >= 2)
            {
                if (numNonPreviousParts >= 4)
                {
                    for (int i = 0; i < argBracket.Count; i++)
                    {
                        if (argBracket[i].LastMatch == true)
                        {
                            temp.Add(argBracket[i]);
                            argBracket.RemoveAt(i);
                        }
                    }
                }

                while (argBracket.Count >= 1 && compRedCheck == false)
                {

                    compRed = PickPlayer(argBracket);

                    //check to see is last match if last match or previous participant is true.
                    //If initial bracket is 3 or less don't check last match status because it won't be possible to avoid last matches then


                    if (compRed.PreviousParticipant == false)
                    {
                        compRedCheck = true;

                    }
                    else
                    {

                        //If a bad competitor is picked add them into a temporary bracket to remove them from the pool
                        temp.Add(compRed);

                    }
                }

                while (argBracket.Count >= 1 && compBlueCheck == false)
                {

                    compBlue = PickPlayer(argBracket);

                    if (compBlue.PreviousParticipant == false)
                    {
                        compBlueCheck = true;

                    }
                    else
                    {
                        temp.Add(compBlue);
                    }
                }

                for (int j = 0; j < temp.Count; j++)
                {

                    argBracket.Add(temp[j]);
                }

                temp.Clear();

                ResetLastMatch(argBracket);

                Match(compRed, compBlue);
                //MatchRedirect(compRed, compBlue);

                //Increment match counter
                match++;

            }
            
            return new RedirectResult("/Events/Index");
        }

        public ActionResult MatchRedirect(Competitor compRed, Competitor compBlue)
        {
            //Asign Match participants by changing model field

            compRed.IsRedComp = true;
            compBlue.IsBlueComp = true;

            //Align local brackets with database brackets

            AlignLocalStateToDB(Winners, Losers, Eliminated);

            return new RedirectResult("/Events/Index");
        }

        public bool MatchMaking(List<Competitor> totalcomps)
        {

                int startingBracket = totalcomps.Count - 1;

                while (Eliminated.Count != startingBracket)
                {

                    //if the Losers bracket is odd then one is selected at random to get a bye and move into the Winners bracket
                    //If the Winners bracket they will be placed into is even they will not have a match this round but if it is odd then they will have a match
                    // This ensures the least amount of byes possible

                    if (Losers.Count % 2 != 0)
                    {
                        int currentPick = bracketPicker.Next(Losers.Count);

                        if (Winners.Count % 2 == 0)
                        {
                            Losers[currentPick].PreviousParticipant = true;
                            Losers[currentPick].Byes++;
                        }

                        Winners.Add(Losers[currentPick]);
                        Losers.RemoveAt(currentPick);
                    }

                    if (Losers.Count != 0)
                    {
                       
                        RunBracket(Losers);
                       
                    }

                RunBracket(Winners);
                
                //Increment round and reset match

                match = 1;
                    round++;

                    ResetParticipant();
                }

            //Add the final winner to the Eliminated bracket. This makes constructing the results output string with proper grammar easier
            Winners[0].Bracket = "Eliminated";
            Eliminated.Add(Winners[0]);

            Winners.Clear();

           //Reverse list and use index as each competitors place this needs to be done out side of the results button so that it isn't reveresed every time it's clicked

            Eliminated.Reverse();

            for(int i = 0; i < Eliminated.Count; i++)
            {
                Eliminated[i].Place = (i + 2);
            }
            
            AlignLocalStateToDB(Winners, Losers, Eliminated);
            return true;

            }
        }
}
