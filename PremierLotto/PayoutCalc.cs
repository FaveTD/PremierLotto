using PremierLotto.Models;
using PremierLotto.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PremierLotto.FInance
{
    public class PayoutCalc
    {
        public static void DistributeEasyPool(List<Player> winners, List<Player> activePlayers, decimal totalJackpotPool)
        {
            Console.WriteLine("--- DISTRIBUTING JACKPOT (PROPORTIONAL SPLIT) ---");

            decimal sumOfWinningStakes = winners.Sum(p => p.ActiveRoundStake);

            foreach (var winner in winners)
            {
                decimal proportionalShare = totalJackpotPool * (winner.ActiveRoundStake / sumOfWinningStakes);
                proportionalShare = Math.Round(proportionalShare, 2);

                winner.TotalWinnings = proportionalShare;

                Console.WriteLine($"Agent {winner.PlayerAlias} won a share of ₦{proportionalShare:N2}!");

                winner.Wallet.DepositWinnings(proportionalShare);
            }
        }

        public static void AwardSingleJackpot(Player champion, decimal totalJackpotPool)
        {
            Console.WriteLine("--- AWARDING JACKPOT TO CHAMPION ---");

            champion.TotalWinnings = totalJackpotPool;

            $"🏆 Champion Agent {champion.PlayerAlias} claims the entire pool of ₦{totalJackpotPool:N2}!".WriteColored(ConsoleColor.Green);

            champion.Wallet.DepositWinnings(totalJackpotPool);
        }

        public decimal CalculateAccuracyWeight(int matches)
        {
            if (matches <= 0) return 0.00m;
            return matches / 4.0m;
        }
    }
}
