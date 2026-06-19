using PremierLotto.FInance;
using System;

namespace PremierLotto.Models
{
    public class PlayerProfile
    {
        public string LegalName { get; set; }
        public string NormalizedName { get; set; }
        public string DisplayName { get; set; }

        public int TotalGamesPlayed { get; set; }
        public int TotalWins { get; set; }
        public int BestScore { get; set; }
        public double AverageScore { get; set; }
        public DateTime FirstSeen { get; set; }
        public DateTime LastSeen { get; set; }

        public Wallet Wallet { get; set; }

        public PlayerProfile()
        {
            Wallet = new Wallet();
        }

        public PlayerProfile(string name)
        {
            DisplayName = name;
            NormalizedName = name.Trim().ToLower();
            LegalName = "";
            FirstSeen = DateTime.Now;
            LastSeen = DateTime.Now;
            TotalGamesPlayed = 0;
            TotalWins = 0;
            BestScore = 0;
            AverageScore = 0.0;
            
            Wallet = new Wallet();
        }

        public void UpdateStats(int roundScore, bool isWin)
        {
            LastSeen = DateTime.Now;

            double totalPointsBefore = AverageScore * TotalGamesPlayed;
            TotalGamesPlayed++;

            AverageScore = Math.Round((totalPointsBefore + roundScore) / TotalGamesPlayed, 2);

            if (roundScore > BestScore)
            {
                BestScore = roundScore;
            }

            if (isWin)
            {
                TotalWins++;
            }
        }
    }
}
