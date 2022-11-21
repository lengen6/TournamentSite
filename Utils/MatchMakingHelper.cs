using Microsoft.Data.SqlClient.Server;
using TieRenTournament.Models;

namespace TieRenTournament.Utils
{
    public class MatchMakingHelper
    {
        //Declare the initial, winners, losers, and eliminated brackets as lists of competitors

        public IList<Competitor> winners = new List<Competitor>();
        public IList<Competitor> losers = new List<Competitor>();
        public IList<Competitor> eliminated = new List<Competitor>();
        public IList<Competitor> temp = new List<Competitor>();


        //Declare global variables

        public Competitor compRed;
        public Competitor compBlue;
        public int round = 1;
        public int match = 1;



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
            Random bracketPicker = new Random();
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
    }
}
