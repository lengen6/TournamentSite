using Microsoft.Data.SqlClient.Server;
using TieRenTournament.Models;

namespace TieRenTournament.Utils
{
    public class MatchMakingHelper
    {
        //Declare the initial, winners, losers, and eliminated brackets as lists of competitors

        public List<Competitor> winners = new List<Competitor>();
        public List<Competitor> losers = new List<Competitor>();
        public List<Competitor> eliminated = new List<Competitor>();
        public List<Competitor> temp = new List<Competitor>();


        //Declare global variables

        public Competitor compRed;
        public Competitor compBlue;
        public int round = 1;
        public int match = 1;

        Random bracketPicker = new Random();

        //Method for resetting previous participant after each round

        public void ResetParticipant()
        {
            foreach (Competitor comp in winners)
            {
                comp.PreviousParticipant = false;
            }

            foreach (Competitor comp in losers)
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

        //Method for determining winners and losers before integration with the scorekeeping app
        //I kept this method in case testing ever needs to be done on the bracket portion of the app in the future
        public void Match(Competitor argcompRed, Competitor argcompBlue)
        {
            //Flip a coin and determine winners based on it. 0 is a win 1 is a loss

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

                winners.Add(argcompRed);

                if (argcompBlue.Losses > 1)
                {
                    eliminated.Add(argcompBlue);
                }
                else
                {
                    losers.Add(argcompBlue);
                }
            }
            else
            {
                argcompBlue.Wins++;
                argcompRed.Losses++;

                winners.Add(argcompBlue);

                if (argcompRed.Losses > 1)
                {
                    eliminated.Add(argcompRed);
                }
                else
                {
                    losers.Add(argcompRed);
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

            //Loop through bracket and determine winners
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

                //Create and show form 2 for the match to occur

                Match(compRed, compBlue);

                //Increment match counter

                match++;


            }

        }

        public List<Competitor> StartMatchMaking(List<Competitor> initial)
        {

                int startingBracket = initial.Count - 1;


                //Check to see if there is an odd number of competitors in the initial bracket. If so randomly remove one and add them to the winners bracket. 
                //This simulates a bye

                if (initial.Count % 2 != 0)
                {
                    int currentPick = bracketPicker.Next(initial.Count);
                    initial[currentPick].Byes++;
                    winners.Add(initial[currentPick]);
                    initial.RemoveAt(currentPick);
                }

                //Go through initial bracket grabbing two competitors at random and adding them to a match

                while (initial.Count > 0)
                {
                    //Grab first player at random assign them to competitor 1 and remove them from the initial bracket

                    compRed = PickPlayer(initial);

                    //Grab second player at random assign them to competitor 2 and remove them from the initial bracket

                    compBlue = PickPlayer(initial);

                    //Now that competitors have been selected and the match is about to occur reset previous participant for everyone left in the bracket

                    if (initial.Count > 0)
                    {
                        ResetLastMatch(initial);
                    }

                    Match(compRed, compBlue);

                //increment match counter match

                match++;                  

                }

                match = 1;
                round++;


                //Reset previous participant status

                ResetParticipant();

                //Run through brackets until all but one competitor is eliminated


                while (eliminated.Count != startingBracket)
                {

                    //if the losers bracket is odd then one is selected at random to get a bye and move into the winners bracket
                    //If the winners bracket they will be placed into is even they will not have a match this round but if it is odd then they will have a match
                    // This ensures the least amount of byes possible

                    if (losers.Count % 2 != 0)
                    {
                        int currentPick = bracketPicker.Next(losers.Count);

                        if (winners.Count % 2 == 0)
                        {
                            losers[currentPick].PreviousParticipant = true;
                            losers[currentPick].Byes++;
                        }

                        winners.Add(losers[currentPick]);
                        losers.RemoveAt(currentPick);
                    }

                    if (losers.Count != 0)
                    {
                        RunBracket(losers);
                    }

                    RunBracket(winners);

                    //Increment round and reset match

                    match = 1;
                    round++;

                    ResetParticipant();
                }

                //Add the final winner to the eliminated bracket. This makes constructing the results output string with proper grammar easier

                eliminated.Add(winners[0]);

                //Reverse list and use index as each competitors place this needs to be done out side of the results button so that it isn't reveresed every time it's clicked

                eliminated.Reverse();
                
                for(int i = 0; i < eliminated.Count; i++)
            {
                eliminated[i].Place = i + 1;
            }

                return eliminated;

            }
        }
}
