using PremierLotto.Core;
using PremierLotto.Models;
using PremierLotto.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PremierLotto.Game
{
    public class LeaderboardManager
    {
        public void DisplayTable(List<Player> players, Dictionary<Player, int> winTracker)
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("ALIAS           | STAKE LEVEL       | TOTAL WINS");
            Console.WriteLine("------------------------------------------------------------------");
            foreach (var p in players)
            {
                Console.WriteLine($"{p.PlayerAlias,-15} | ₦{p.ActiveRoundStake,-15:N2} | {winTracker[p]}");
            }
            Console.WriteLine("------------------------------------------------------------------");
        }
    }
}
