using PremierLotto.Models;
using PremierLotto.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PremierLotto.Finance
{
    public static class PayoutCalc 
    {
        public static void DistributeEasyPool(List<Player> winners, List<Player> activePlayers, decimal totalJackpotPool)
        {
            Console.WriteLine("\n--- DISTRIBUTING JACKPOT ---");

            decimal sumOfWinningStakes = winners.Sum(p => p.ActiveRoundStake);

            foreach (var winner in winners)
            {
                if (sumOfWinningStakes == 0) continue;

                decimal proportionalShare = totalJackpotPool * (winner.ActiveRoundStake / sumOfWinningStakes);
                proportionalShare = Math.Round(proportionalShare, 2);

                winner.TotalWinnings = proportionalShare;

                ($"Agent {winner.PlayerAlias} won a share of ₦{proportionalShare:N2}!").WriteColored(ConsoleColor.Green);

                winner.Wallet.DepositWinnings(proportionalShare);
            }
        }

        public static void AwardSingleJackpot(Player champion, decimal totalJackpotPool)
        {
            Console.WriteLine("\n--- AWARDING JACKPOT TO CHAMPION ---");

            champion.TotalWinnings = totalJackpotPool;

            $"🏆 Champion Agent {champion.PlayerAlias} claims the entire pool of ₦{totalJackpotPool:N2}!".WriteColored(ConsoleColor.Green);

            champion.Wallet.DepositWinnings(totalJackpotPool);
        }

        public static decimal CalculateAccuracyWeight(int matches) 
        {
            if (matches <= 0) return 0.00m;
            return matches / 4.0m;
        }
    }
}
