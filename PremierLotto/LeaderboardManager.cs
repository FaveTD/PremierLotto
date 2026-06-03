using System;
using System.Collections.Generic;
using System.Linq;

namespace PremierLotto
{
    public class LeaderboardManager
    {
        public List<Player> GetSortedLeaderboard(List<Player> players)
        {
            return players.OrderByDescending(p => p.CorrectMatches)
                          .ThenByDescending(p => p.TotalWinnings)
                          .ToList();
        }

        public void DisplayTable(List<Player> sortedPlayers)
        {
            Console.WriteLine("\n-------------------------------------------------------------");
            Console.WriteLine(string.Format("{0,-15} | {1,-10} | {2,-10} | {3,-10}", "ALIAS", "MATCHES", "WINNINGS", "RANK"));
            Console.WriteLine("-------------------------------------------------------------");

            int rank = 1;
            foreach (var p in sortedPlayers)
            {
                Console.WriteLine(string.Format("{0,-15} | {1,-10} | ₦{2,-9} | #{3}",
                    p.PlayerAlias, p.CorrectMatches, p.TotalWinnings, rank++));
            }
            Console.WriteLine("-------------------------------------------------------------");
        }

        public List<Player> GetTiedWinners(List<Player> sortedPlayers)
        {
            if (sortedPlayers.Count < 2) return new List<Player>();

            int topScore = sortedPlayers[0].CorrectMatches;

            return sortedPlayers.Where(p => p.CorrectMatches == topScore).ToList();
        }

        public void RunTieBreaker(List<Player> tiedPlayers, InputHandler input, Validation validator, GameSettings settings, MatchCheck checker)
        {
            Console.WriteLine("\n--- SUDDEN DEATH: ROLLUP PROTOCOL ---");

            foreach (var p in tiedPlayers)
            {
                Console.WriteLine($"\n{p.PlayerAlias}, enter your final tie-breaker guesses:");
                p.Guesses = input.GetConfirmedGuesses(validator, settings);
            }

            LottoEngine tieDraw = new LottoEngine(settings);
            Console.WriteLine($"\nFinal Winning Numbers: {string.Join(" | ", tieDraw.WinningNumbers)}");

            foreach (var p in tiedPlayers)
            {
                int matches = checker.GetMatchCount(p.Guesses, tieDraw.WinningNumbers);
                Console.WriteLine($"{p.PlayerAlias} matched {matches} numbers.");
                p.CorrectMatches += matches; 
            }
        }
        public void HandlePotentialTies(List<Player> sortedResults, GameSettings settings)
        {
            if (settings.HasRollup)
            {
                var tied = GetTiedWinners(sortedResults);
                if (tied.Count > 1)
                {
                    "🚨 TIE DETECTED! INITIATING ROLLUP PROTOCOL...".WriteColored(ConsoleColor.Red);
                    Thread.Sleep(2000);

                    RunTieBreaker(tied, new InputHandler(), new Validation(), settings, new MatchCheck());

                    "--- FINAL STANDINGS AFTER SUDDEN DEATH ---".WriteCentered(ConsoleColor.Yellow);
                    var finalResults = GetSortedLeaderboard(sortedResults);
                    DisplayTable(finalResults);
                }
            }
        }

    }
}
