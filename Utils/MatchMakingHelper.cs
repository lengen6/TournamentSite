using Microsoft.Data.SqlClient.Server;
using TieRenTournament.Models;

namespace TieRenTournament.Utils
{
    public class MatchMakingHelper
    {
        private static TieRenTournament.Data.ApplicationDbContext _context;

        public MatchMakingHelper(TieRenTournament.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Initial> Initial { get; set; }
        public List<Winner> Winners { get; set; }
        public List<Loser> Losers { get; set; }
        public List<Eliminated> Eliminated { get; set; }
        public List<Competitor> temp = new List<Competitor>();

        public MatchMakingHelper(List<Initial> initial, List<Winner> winners, List<Loser> losers, List<Eliminated> eliminated){
            Initial = initial;
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
        public void Match(Competitor argcompRed, Competitor argcompBlue)
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

                Winners.Add((Winner)argcompRed);

                if (argcompBlue.Losses > 1)
                {
                    Eliminated.Add((Eliminated)argcompBlue);
                }
                else
                {
                    Losers.Add((Loser)argcompBlue);
                }
            }
            else
            {
                argcompBlue.Wins++;
                argcompRed.Losses++;

                Winners.Add((Winner)argcompBlue);

                if (argcompRed.Losses > 1)
                {
                    Eliminated.Add((Eliminated)argcompRed);
                }
                else
                {
                    Losers.Add((Loser)argcompRed);
                }
            }

        }

        //Method for running through a bracket

        public void RunBracket(List<Competitor> argBracket)
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

                //Align local brackets with database brackets by dropping the tables and setting them to the local brackets

                Match(compRed, compBlue);

                //Increment match counter

                match++;


            }

        }

        public async void StartMatchmaking(List<Competitor> initial)
        {
            foreach(Winner competitor in initial)
            {
                _context.Competitor.Attach(competitor);
            }

            await _context.SaveChangesAsync();
        }

        public bool ContinueMatchMaking(List<Competitor> totalcomps)
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

                        Competitor compToSwitch = Losers[currentPick];

                        Winners.Add((Winner)compToSwitch);
                        Losers.RemoveAt(currentPick);
                    }

                    if (Losers.Count != 0)
                    {
                        List<Competitor> tempLosers = new List<Competitor>();
                        foreach (Competitor comp in Losers)
                        {
                            tempLosers.Add(comp);
                        }

                        RunBracket(tempLosers);
                        Losers.Clear();

                        foreach(Loser loser in tempLosers)
                        {
                            Losers.Add(loser);
                        }
                    }

                List<Competitor> tempWinners = new List<Competitor>();
                foreach (Competitor comp in Winners)
                {
                    tempWinners.Add(comp);
                }

                RunBracket(tempWinners);
                Winners.Clear();

                foreach (Winner winner in tempWinners)
                {
                    Winners.Add(winner);
                }

                //Increment round and reset match

                match = 1;
                    round++;

                    ResetParticipant();
                }

            //Add the final winner to the Eliminated bracket. This makes constructing the results output string with proper grammar easier


                Competitor compToSwitchTwo = Winners[0];

                Eliminated.Add((Eliminated)compToSwitchTwo);

           //Reverse list and use index as each competitors place this needs to be done out side of the results button so that it isn't reveresed every time it's clicked

                Eliminated.Reverse();
                
           //*****TODO Update all tables

                return true;

            }
        }
}
