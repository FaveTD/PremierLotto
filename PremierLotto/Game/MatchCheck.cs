using PremierLotto.Core;
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
            const int totalBallsPerRound = 4;
            double matchPercentage = (double)matches / totalBallsPerRound;

            return matchPercentage >= settings.Config.PassThreshold;
        }
    }
}