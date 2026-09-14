using PremierLotto.Core;
using PremierLotto.Models;
using PremierLotto.Utilities;
using System;
using System.Collections.Generic;

namespace PremierLotto.Game
{
    public class LeaderboardManager
    {
        public void DisplayTable(List<Player> players, Dictionary<Player, int> winTracker)
        {
            Console.Clear();
            "============================================================".WriteCentered(ConsoleColor.Cyan);
            "                   TOURNAMENT LEADERBOARD                   ".WriteCentered(ConsoleColor.Cyan);
            "============================================================".WriteCentered(ConsoleColor.Cyan);

            
            Console.WriteLine($"{"ALIAS",-15} | {"STAKE",-12} | {"WINS",-6}");
            Console.WriteLine("------------------------------------------------------------");

            foreach (var p in players)
            {
                if (winTracker.ContainsKey(p))
                {
                    Console.WriteLine($"{p.PlayerAlias,-15} | ₦{p.ActiveRoundStake,-10:N2} | {winTracker[p],-6}");
                }
            }
            Console.WriteLine("------------------------------------------------------------");

            
            Console.WriteLine("\nPress ANY KEY to see payout details...");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}
