using PremierLotto.Core;
using PremierLotto.Finance;
using PremierLotto.Models;
using PremierLotto.Utilities;
using PremierLotto.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PremierLotto.Game
{
    public static class TournamentManager
    {
        public static void RunTournament(List<Player> players, GameSettings settings, PoolManager poolManager)
        {
            FinanceManager finance = new FinanceManager();

            Console.Clear();
            "┌────────────────────────────────────────────────────────┐".WriteCentered(ConsoleColor.Cyan);
            "│              ► TOURNAMENT INITIALIZATION ◄             │".WriteCentered(ConsoleColor.Cyan);
            "└────────────────────────────────────────────────────────┘".WriteCentered(ConsoleColor.Cyan);

            Dictionary<Player, int> winTracker = new Dictionary<Player, int>();
            List<Player> activePlayers = new List<Player>();
            InputHandler input = new InputHandler();
            Validation validator = new Validation();

            LeaderboardManager leaderboard = new LeaderboardManager();

            foreach (var p in players) p.TotalWinnings = 0;

            foreach (var player in players)
            {
                Console.Clear();
                Console.WriteLine($"--- Agent {player.PlayerAlias} ---");
                decimal userStake = input.GetValidatedStake(player);

                if (poolManager.ProcessStake(userStake, player.Wallet))
                {
                    player.ActiveRoundStake = userStake;
                    activePlayers.Add(player);
                    winTracker[player] = 0;

                    finance.LogTransaction("Debit", player.PlayerAlias, userStake, "Tournament Entry Stake");

                    Console.WriteLine($"✅ Stake of ₦{userStake:N2} processed. (10% House Fee deducted).");
                }
                else
                {
                    Console.WriteLine("❌ Entry denied: Insufficient funds.");
                }
                Console.ReadKey();
            }

            if (activePlayers.Count < 1)
            {
                "❌ Aborting Tournament: No players qualified.".WriteColored(ConsoleColor.Red);
                while (Console.KeyAvailable) Console.ReadKey(true);
                Console.ReadKey(true);
                return;
            }

            " 🔒 All agent initialization stakes compiled and locked.".WriteColored(ConsoleColor.Yellow);
            while (Console.KeyAvailable) Console.ReadKey(true);
            Console.ReadKey(true);

            for (int round = 1; round <= settings.NumberOfRounds; round++)
            {
                Dictionary<Player, int> roundMatches = new Dictionary<Player, int>();
                foreach (var player in activePlayers)
                {
                    Console.Clear();
                    $"============ ROUND {round} OF {settings.NumberOfRounds} ============".WriteCentered(ConsoleColor.Yellow);
                    Console.WriteLine($"\n--- Agent {player.PlayerAlias}'s Turn ---");
                    player.Guesses = input.GetConfirmedGuesses(validator, settings);
                }

                Console.Clear();
                "► SYSTEM DRAW INITIATED ◄".WriteCentered(ConsoleColor.Cyan);
                LottoEngine roundEngine = new LottoEngine(settings);
                List<string> winningNumbers = roundEngine.WinningNumbers;
                Console.WriteLine($"\n⚡ DRAW: {string.Join(" | ", winningNumbers)}\n");

                foreach (var player in activePlayers)
                {
                    int matches = MatchCheck.CountMatches(player.Guesses, winningNumbers);
                    roundMatches[player] = matches;
                    if (matches > 0) $"Agent {player.PlayerAlias} you have {matches} match(es)".WriteColored(ConsoleColor.Green);
                    else "Oof...sorry. Try again in the next round".WriteColored(ConsoleColor.DarkGray);
                }

                int maxRoundMatches = roundMatches.Values.Max();
                if (maxRoundMatches > 0)
                {
                    var roundLeaders = roundMatches.Where(x => x.Value == maxRoundMatches).Select(x => x.Key).ToList();
                    foreach (var leader in roundLeaders) winTracker[leader]++;
                    if (roundLeaders.Count > 1) "🤝 Tie detected!".WriteColored(ConsoleColor.Yellow);
                    else $"🏆 {roundLeaders[0].PlayerAlias} led the round!".WriteColored(ConsoleColor.Cyan);
                }
                else "🚫 No one matched any balls this round.".WriteColored(ConsoleColor.DarkGray);

                Console.WriteLine("\nPress ANY KEY to continue...");
                while (Console.KeyAvailable) Console.ReadKey(true);
                Console.ReadKey(true);
            }

            leaderboard.DisplayTable(activePlayers, winTracker);

            decimal totalJackpotPool = poolManager.GetCurrentPool();
            int maxWins = winTracker.Values.Max();

            if (maxWins == 0)
            {
                Console.Clear();
                "====================================================".WriteCentered(ConsoleColor.Yellow);
                "          ► JACKPOT ACCUMULATION PROTOCOL ◄         ".WriteCentered(ConsoleColor.Cyan);
                "====================================================".WriteCentered(ConsoleColor.Yellow);
                Console.WriteLine("\nNo players reached the success threshold in this session.");
                Console.WriteLine("The current jackpot has been carried forward to the next tournament.");
                Console.WriteLine($"\nCarried Pool Balance: ₦{totalJackpotPool:N2}");
            }
            else
            {
                List<Player> winners = winTracker.Where(x => x.Value == maxWins).Select(x => x.Key).ToList();

                if (settings.ModeName != "Easy" && winners.Count > 1)
                {
                    "⚡ TIE DETECTED! Sudden Death initiated...".WriteColored(ConsoleColor.Red);
                    Player singleChampion = RunSuddenDeath(winners, settings, input, validator);
                    PayoutCalc.AwardSingleJackpot(singleChampion, totalJackpotPool);

                    finance.LogTransaction("Credit", singleChampion.PlayerAlias, totalJackpotPool, "Sudden Death Payout");
                }
                else
                {
                    PayoutCalc.DistributeEasyPool(winners, activePlayers, totalJackpotPool);

                    foreach (var winner in winners)
                    {
                        finance.LogTransaction("Credit", winner.PlayerAlias, winner.TotalWinnings, "Tournament Payout");
                    }
                }

                HistoryManager history = new HistoryManager();
                history.AppendTournamentLog(settings.ModeName, totalJackpotPool, activePlayers, winTracker);

                poolManager.ResetPool();
            }

            Console.WriteLine("\nPress ANY KEY to exit...");
            while (Console.KeyAvailable) Console.ReadKey(true);
            Console.ReadKey(true);
        }

        private static Player RunSuddenDeath(List<Player> tiedPlayers, GameSettings settings, InputHandler input, Validation validator)
        {
            List<Player> currentCandidates = new List<Player>(tiedPlayers);

            while (currentCandidates.Count > 1)
            {
                Console.WriteLine($"\n⚡{currentCandidates.Count} players are still tied. Another round required!");

                Dictionary<Player, int> roundScores = new Dictionary<Player, int>();

                foreach (var player in currentCandidates)
                {
                    Console.WriteLine($"\n--- {player.PlayerAlias}: Sudden Death Round ---");
                    player.Guesses = input.GetConfirmedGuesses(validator, settings);
                }

                LottoEngine engine = new LottoEngine(settings);
                List<string> winningNumbers = engine.WinningNumbers;
                Console.WriteLine($"\n⚡ Draw: {string.Join(" | ", winningNumbers)}");

                int maxMatches = 0;
                foreach (var player in currentCandidates)
                {
                    int matches = MatchCheck.CountMatches(player.Guesses, winningNumbers);
                    roundScores[player] = matches;
                    if (matches > maxMatches) maxMatches = matches;
                    Console.WriteLine($"{player.PlayerAlias} scored {matches} match(es).");
                }

                currentCandidates = roundScores.Where(x => x.Value == maxMatches)
                                               .Select(x => x.Key)
                                               .ToList();
            }

            Console.WriteLine($"\n🏆 Sudden Death Champion: {currentCandidates[0].PlayerAlias}");
            return currentCandidates[0];
        }
    }
}
