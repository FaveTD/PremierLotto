using PremierLotto.Core;
using PremierLotto.FInance;
using PremierLotto.Models;
using PremierLotto.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PremierLotto.Game
{
    public static class TournamentManager
    {
        public static void RunTournament(List<Player> players, GameSettings settings)
        {
            Console.Clear();
            "┌────────────────────────────────────────────────────────┐".WriteCentered(ConsoleColor.Cyan);
            "│              ► TOURNAMENT INITIALIZATION ◄             │".WriteCentered(ConsoleColor.Cyan);
            "└────────────────────────────────────────────────────────┘".WriteCentered(ConsoleColor.Cyan);

            decimal totalJackpotPool = 0;
            Dictionary<Player, int> winTracker = new Dictionary<Player, int>();

            Console.WriteLine("\nAgents, initialize entry configuration protocols:\n");
            foreach (var player in players)
            {
                Console.WriteLine($"--- Agent {player.PlayerAlias} ---");
                decimal validatedBaseStake = player.Wallet.ProcessRoundStake();

                if (validatedBaseStake <= 0)
                {
                    player.ActiveRoundStake = 0;
                    continue;
                }

                player.ActiveRoundStake = validatedBaseStake;
                totalJackpotPool += validatedBaseStake;
                winTracker[player] = 0;
                Console.WriteLine();
            }

            var activePlayers = players.Where(p => p.ActiveRoundStake > 0).ToList();

            if (activePlayers.Count < 1)
            {
                "❌ Aborting Tournament: Not enough qualified players with active stakes.".WriteColored(ConsoleColor.Red);
                Console.ReadKey();
                return;
            }

            " 🔒 All agent initialization stakes compiled and locked.".WriteColored(ConsoleColor.Yellow);
            "Press ANY KEY to launch the tournament rounds...".WriteColored(ConsoleColor.DarkGray);
            Console.ReadKey();

            InputHandler input = new InputHandler();
            Validation validator = new Validation();

            int totalRounds = settings.NumberOfRounds;
            for (int round = 1; round <= totalRounds; round++)
            {
                for (int i = 0; i < activePlayers.Count; i++)
                {
                    var player = activePlayers[i];
                    Console.Clear();
                    $"============ ROUND {round} OF {totalRounds} ============".WriteCentered(ConsoleColor.Yellow);
                    Console.WriteLine($"\n--- Agent {player.PlayerAlias}'s Turn ---");

                    player.Guesses = input.GetConfirmedGuesses(validator, settings);
                }

                Console.Clear();
                "┌────────────────────────────────────────────────────────┐".WriteCentered(ConsoleColor.Cyan);
                "│               ► SYSTEM DRAW INITIATED ◄                │".WriteCentered(ConsoleColor.Cyan);
                "└────────────────────────────────────────────────────────┘".WriteCentered(ConsoleColor.Cyan);

                LottoEngine roundEngine = new LottoEngine(settings);
                List<string> winningNumbers = roundEngine.WinningNumbers;

                Console.WriteLine("\n------------------------------------------------");
                Console.Write("⚡ SYSTEM DRAW COMPLETE | Round Winning Matrix: ");
                Console.WriteLine(string.Join(" | ", winningNumbers));
                Console.WriteLine("------------------------------------------------\n");

                Dictionary<Player, int> successfulRoundMatches = new Dictionary<Player, int>();

                foreach (var player in activePlayers)
                {
                    int matches = MatchCheck.CountMatches(player.Guesses, winningNumbers);
                    bool passedThreshold = MatchCheck.EvaluatePassTarget(matches, settings);

                    if (passedThreshold)
                    {
                        $"✨ Threshold Passed! Agent {player.PlayerAlias} matched {matches} ball(s).".WriteColored(ConsoleColor.Green);
                        successfulRoundMatches[player] = matches;
                    }
                    else
                    {
                        $"❌ Threshold Failed for Agent {player.PlayerAlias} ({matches} match(es)).".WriteColored(ConsoleColor.DarkGray);
                    }
                }

                if (successfulRoundMatches.Count > 0)
                {
                    int maxRoundMatches = successfulRoundMatches.Values.Max();
                    var roundWinners = successfulRoundMatches.Where(x => x.Value == maxRoundMatches).Select(x => x.Key).ToList();

                    Console.WriteLine();
                    if (roundWinners.Count > 1)
                    {
                        string tieNames = string.Join(" and ", roundWinners.Select(w => $"Agent {w.PlayerAlias}"));
                        $"🤝 TIE ROUND! {tieNames} won Round {round}!".WriteColored(ConsoleColor.Yellow);
                    }
                    else
                    {
                        $"🏆 Agent {roundWinners[0].PlayerAlias} won Round {round}!".WriteColored(ConsoleColor.Cyan);
                    }

                    foreach (var winner in roundWinners)
                    {
                        winTracker[winner]++;
                    }
                }
                else
                {
                    Console.WriteLine("\n🚫 No agents managed to pass the matrix threshold this round.");
                }

                Console.WriteLine("\nPress ANY KEY to advance the tournament sequence...");
                Console.ReadKey();
            }

            Console.Clear();
            "┌────────────────────────────────────────────────────────┐".WriteCentered(ConsoleColor.Cyan);
            "│               ► FINAL TOURNAMENT SCORECARD ◄           │".WriteCentered(ConsoleColor.Cyan);
            "└────────────────────────────────────────────────────────┘".WriteCentered(ConsoleColor.Cyan);

            "┌────────────────────────┬───────────────────────────────┐".WriteCentered(ConsoleColor.DarkGray);
            "│ AGENT ALIAS            │ TOTAL ROUND WINS              │".WriteCentered(ConsoleColor.DarkGray);
            "├────────────────────────┼───────────────────────────────┤".WriteCentered(ConsoleColor.DarkGray);

            foreach (var player in activePlayers)
            {
                int score = winTracker.ContainsKey(player) ? winTracker[player] : 0;
                string row = $"│ {player.PlayerAlias,-22} │ {score,-29} │";
                row.WriteCentered(ConsoleColor.White);
            }
            "└────────────────────────┴───────────────────────────────┘".WriteCentered(ConsoleColor.DarkGray);

            Console.WriteLine($"\nTotal Prize Jackpot Pool: ₦{totalJackpotPool:N2}\n");

            int maxWins = winTracker.Values.Max();

            if (maxWins == 0)
            {
                "🚫 No rounds were won by any agent! The system pool rolls up.".WriteColored(ConsoleColor.Yellow);
                Console.ReadKey();
                return;
            }

            List<Player> tournamentWinners = winTracker.Where(x => x.Value == maxWins).Select(x => x.Key).ToList();

            if (settings.ModeName == "Easy")
            {
                PayoutCalc.DistributeEasyPool(tournamentWinners, activePlayers, totalJackpotPool);
            }
            else
            {
                if (tournamentWinners.Count > 1)
                {
                    "⚡ TIE DETECTED IN OVERALL SCORE! Initiating Sudden Death Protocol...".WriteColored(ConsoleColor.Red);
                    Player singleChampion = RunSuddenDeath(tournamentWinners, settings, input, validator);
                    PayoutCalc.AwardSingleJackpot(singleChampion, totalJackpotPool);
                }
                else
                {
                    PayoutCalc.AwardSingleJackpot(tournamentWinners[0], totalJackpotPool);
                }
            }

            Console.WriteLine("\nPress ANY KEY to exit session debrief and save log files...");
            Console.ReadKey();
        }

        private static Player RunSuddenDeath(List<Player> tiedPlayers, GameSettings settings, InputHandler input, Validation validator)
        {
            while (true)
            {
                Console.Clear();
                "--- SUDDEN DEATH ELIMINATION ROUND ---".WriteCentered(ConsoleColor.Red);
                Dictionary<Player, int> suddenDeathScores = new Dictionary<Player, int>();

                foreach (var player in tiedPlayers)
                {
                    Console.WriteLine($"\n--- Agent {player.PlayerAlias}'s Sudden Death Guess ---");
                    player.Guesses = input.GetConfirmedGuesses(validator, settings);
                }

                LottoEngine suddenEngine = new LottoEngine(settings);
                List<string> winningNumbers = suddenEngine.WinningNumbers;

                Console.Clear();
                "--- SUDDEN DEATH DRAW RESULTS ---".WriteCentered(ConsoleColor.Red);
                Console.WriteLine("\n------------------------------------------------");
                Console.Write("⚡ SUDDEN DEATH DRAW: ");
                Console.WriteLine(string.Join(" | ", winningNumbers));
                Console.WriteLine("------------------------------------------------\n");

                foreach (var player in tiedPlayers)
                {
                    int matches = MatchCheck.CountMatches(player.Guesses, winningNumbers);
                    Console.WriteLine($"Agent {player.PlayerAlias} achieved {matches} match(es).");
                    suddenDeathScores[player] = matches;
                }

                int highestScore = suddenDeathScores.Values.Max();
                var champions = suddenDeathScores.Where(x => x.Value == highestScore).Select(x => x.Key).ToList();

                if (champions.Count == 1)
                {
                    $"🏆 Tie Broken! Champion is Agent {champions[0].PlayerAlias}!".WriteColored(ConsoleColor.Green);
                    return champions[0];
                }

                "Still tied! Re-rolling sudden death matrix...".WriteColored(ConsoleColor.Yellow);
                Console.ReadKey();
            }
        }
    }
}
