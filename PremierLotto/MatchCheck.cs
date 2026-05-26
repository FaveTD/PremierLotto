using System;
using System.Collections.Generic;
using System.Text;

namespace PremierLotto
{
    public class MatchCheck
    {
        public int GetMatchCount(List<string> userGuesses, List<string> winningNumbers)
        {
            return userGuesses.Intersect(winningNumbers).Count();
        }
    }
}
