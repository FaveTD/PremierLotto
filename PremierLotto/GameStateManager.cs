using System;

namespace PremierLotto
{
    public class GameStateManager
    {
        public int CorrectMatches { get; private set; }

        public bool UpdateAndEvaluateRound(int matchesThisRound, int currentTrials)
        {
            if (matchesThisRound > CorrectMatches)
            {
                CorrectMatches = matchesThisRound;
            }

            Console.WriteLine($"Matched {matchesThisRound} ball(s)!");

            if (matchesThisRound == 4)
            {
                Console.WriteLine("\n🏆 JACKPOT! 🏆");
                return true; 
            }

            if (currentTrials == 0)
            {
                Console.WriteLine("\nGAME OVER. You have no more trials left. Better luck next time!");
            }
            else
            {
                Console.WriteLine($"Not quite the jackpot. You have {currentTrials} trial(s) left. Let's try again!");
            }

            return false; 
        }
    }
}