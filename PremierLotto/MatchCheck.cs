using PremierLotto.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PremierLotto.Game
{
    public class MatchCheck
    {
        public static int CountMatches(List<string> userGuesses, List<string> winningNumbers)
        {
            if (userGuesses == null || winningNumbers == null) return 0;
            return userGuesses.Intersect(winningNumbers).Count();
        }

        public static bool EvaluatePassTarget(int matches, GameSettings settings)
        {
            int totalBallsPerRound = 4;
            double matchPercentage = (double)matches / totalBallsPerRound;

            if (settings.ModeName == "Easy")
            {
                return matchPercentage >= 0.25; 
            }
            if (settings.ModeName == "Classic")
            {
                return matchPercentage >= 0.50; 
            }
            if (settings.ModeName == "Pro")
            {
                return matchPercentage >= 0.75; 
            }

            return false;
        }
    }
}
