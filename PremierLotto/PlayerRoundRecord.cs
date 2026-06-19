using System;
using System.Collections.Generic;

namespace PremierLotto.Models
{
    public class PlayerRoundRecord
    {
        public string PlayerAlias { get; set; }
        public List<string> PlayerGuesses { get; set; }
        public int MatchesCount { get; set; }
        public decimal WinningsClaimed { get; set; }

        public PlayerRoundRecord() { }

        public PlayerRoundRecord(string alias, List<string> guesses, int matches, decimal winnings)
        {
            PlayerAlias = alias;
            PlayerGuesses = guesses != null ? new List<string>(guesses) : new List<string>();
            MatchesCount = matches;
            WinningsClaimed = winnings;
        }
    }
}
