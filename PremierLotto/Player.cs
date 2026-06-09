using System;
using System.Collections.Generic;

namespace PremierLotto
{
    public class Player
    {
        public string RealName { get; set; }
        public string PlayerAlias { get; set; } 

        public List<string> Guesses { get; set; }

        public int CorrectMatches { get; set; }
        public decimal TotalWinnings { get; set; }
        
        public bool IsWinnerOfRound { get; set; }

        public Player(string realName, string alias)
        {
            RealName = realName;
            PlayerAlias = alias;
            Guesses = new List<string>();
            CorrectMatches = 0;
            TotalWinnings = 0;
            IsWinnerOfRound = false; 
        }
    }
}
