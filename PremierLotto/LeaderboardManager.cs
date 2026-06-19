using PremierLotto.Core;
using PremierLotto.Models;
using PremierLotto.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PremierLotto.Game
{
    public class LeaderboardManager
    {
        public List<Player> GetSortedLeaderboard(List<Player> players)
        {
            return players.OrderByDescending(p => p.TotalWinnings) 
                          .ThenByDescending(p => p.PlayerAlias)    
                          .ToList();
        }

        public void DisplayTable(List<Player> sortedPlayers)
        {
            Console.WriteLine("\n-------------------------------------------------------------");
            Console.WriteLine(string.Format("{0,-15} | {1,-15} | {2,-15}", "ALIAS", "STAKE LEVEL", "TOTAL WINNINGS"));
            Console.WriteLine("-------------------------------------------------------------");

            int rank = 1;
            foreach (var p in sortedPlayers)
            {
                Console.WriteLine(string.Format("{0,-15} | ₦{1,-13:N2} | ₦{2,-13:N2} | #{3}",
                    p.PlayerAlias, p.ActiveRoundStake, p.TotalWinnings, rank++));
            }
            Console.WriteLine("-------------------------------------------------------------");
        }

        public List<Player> GetTiedWinners(List<Player> sortedPlayers)
        {
            if (sortedPlayers.Count < 2) return new List<Player>();

            decimal topWinnings = sortedPlayers[0].TotalWinnings;

            return sortedPlayers.Where(p => p.TotalWinnings == topWinnings).ToList();
        }

        public void RunTieBreaker(List<Player> tiedPlayers, GameSettings settings)
        {
            Console.WriteLine("\n--- SUDDEN DEATH: ROLLUP PROTOCOL ---");

            foreach (var p in tiedPlayers)
            {
                Console.WriteLine($"\n{p.PlayerAlias}, enter your 4 final tie-breaker guesses:");

                List<string> tieGuesses = new List<string>();
                for (int g = 1; g <= 4; g++)
                {
                    Console.Write($"Prediction {g}: ");
                    tieGuesses.Add(Console.ReadLine()?.Trim());
                }
                p.Guesses = tieGuesses;
            }

            LottoEngine tieDraw = new LottoEngine(settings);
            Console.WriteLine($"\nFinal Winning Numbers: {string.Join(" | ", tieDraw.WinningNumbers)}");

            foreach (var p in tiedPlayers)
            {
                int matches = MatchCheck.CountMatches(p.Guesses, tieDraw.WinningNumbers);
                Console.WriteLine($"{p.PlayerAlias} matched {matches} numbers.");
            }
        }

        public void HandlePotentialTies(List<Player> sortedResults, GameSettings settings)
        {
            if (!settings.HasRollup)
            {
                Console.WriteLine("\nRollup protocol is not supported for this game mode. Standings are final.");
                return;
            }

            var tied = GetTiedWinners(sortedResults);
            if (tied.Count > 1)
            {
                "🚨 TIE DETECTED! INITIATING ROLLUP PROTOCOL...".WriteColored(ConsoleColor.Red);
                Thread.Sleep(2000);

                RunTieBreaker(tied, settings);

                "--- FINAL STANDINGS AFTER SUDDEN DEATH ---".WriteCentered(ConsoleColor.Yellow);
                var finalResults = GetSortedLeaderboard(sortedResults);
                DisplayTable(finalResults);
            }
        }

        public void DisplayFinalResults(List<Player> players, GameSettings settings)
        {
            Console.Clear();
            "*************************************************".WriteCentered(ConsoleColor.Green);
            "           FINAL MISSION DEBRIEF                ".WriteCentered(ConsoleColor.Green);
            "*************************************************".WriteCentered(ConsoleColor.Green);

            var sortedResults = GetSortedLeaderboard(players);
            DisplayTable(sortedResults);

            HandlePotentialTies(sortedResults, settings);

            "SESSION COMPLETE. LOGGING OUT.".WriteCentered(ConsoleColor.Yellow);
            Thread.Sleep(2000);
        }
    }
}
