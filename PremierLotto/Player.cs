using PremierLotto.FInance;
using System;
using System.Collections.Generic;

namespace PremierLotto.Models
{
    public class Player
    {
        public string RealName { get; set; }
        public string PlayerAlias { get; set; }

        public List<string> Guesses { get; set; }

        public int CorrectMatches { get; set; }
        public decimal TotalWinnings { get; set; }

        public bool IsWinnerOfRound { get; set; }

        public Wallet Wallet { get; private set; }
        public decimal ActiveRoundStake { get; set; }

        public Player(string realName, string alias, Wallet profileWallet)
        {
            RealName = realName;
            PlayerAlias = alias;
            Guesses = new List<string>();
            CorrectMatches = 0;
            TotalWinnings = 0;
            IsWinnerOfRound = false;

            Wallet = profileWallet;
            ActiveRoundStake = 0.00m;
        }
    }
}
