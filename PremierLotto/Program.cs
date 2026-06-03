using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PremierLotto
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Globus Bank - Premier Lotto Terminal";

            "***************************************".WriteCentered(ConsoleColor.Yellow);
            "** WELCOME TO PREMIER LOTTO 🤞       **".WriteCentered(ConsoleColor.Yellow);
            "***************************************".WriteCentered(ConsoleColor.Yellow);

            int age;
            Console.WriteLine("\nAgent, enter your age to access the terminal:");
            if (!Console.ReadLine().IsValidAge(out age)) return;

            "Access Granted. Initializing System...".AnimatedWrite(40);
            Thread.Sleep(1000);

            GameOption selectedOption = GameSettings.ShowMenuAndSelect();

            decimal userStake = GameStart.GetUserStake();

            Console.Write("\nEnable Duplicate Numbers in Guesses? (Y/N): ");
            bool allowDupes = Console.ReadLine()?.ToUpper() == "Y";

            bool allowAlpha = false;
            if (selectedOption.Name == "Pro")
            {
                Console.Write("Activate Alphanumeric Mode (A1, B2...)? (Y/N): ");
                allowAlpha = Console.ReadLine()?.ToUpper() == "Y";
            }

            GameSettings settings = new GameSettings(selectedOption, allowDupes, allowAlpha);

            Validation validator = new Validation();
            List<Player> playersList = new List<Player>();
            GameStart.RegisterAgents(validator, playersList);

            RunTournament(playersList, settings, userStake);
            DisplayFinalResults(playersList, settings);
        }

        static void RunTournament(List<Player> players, GameSettings settings, decimal stake)
        {
            InputHandler inputSec = new InputHandler();
            Validation validator = new Validation();

            for (int r = 1; r <= settings.NumberOfRounds; r++)
            {
                Console.Clear();
                "=======================================".WriteCentered(ConsoleColor.Cyan);
                $"       TOURNAMENT ROUND {r} OF {settings.NumberOfRounds}".WriteCentered(ConsoleColor.Cyan);
                "=======================================".WriteCentered(ConsoleColor.Cyan);

                foreach (var player in players)
                {
                    Console.WriteLine($"\nAgent [{player.PlayerAlias.ToUpper()}], prepare your guesses.");
                    player.Guesses = inputSec.GetConfirmedGuesses(validator, settings);
                }

                LottoEngine draw = new LottoEngine(settings);
                "SHUFFLING DIGITAL DRUM...".AnimatedWrite(60);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nWINNING COMBINATION: [ {string.Join(" | ", draw.WinningNumbers)} ]");
                Console.ResetColor();

                TournamentManager.ProcessPlayerResults(players, draw.WinningNumbers, settings, stake);

                if (r < settings.NumberOfRounds)
                {
                    Console.WriteLine("\nRound Complete. Press any key to proceed...");
                    Console.ReadKey();
                }
            }
        }

        static void DisplayFinalResults(List<Player> players, GameSettings settings)
        {
            LeaderboardManager boardManager = new LeaderboardManager();
            Console.Clear();
            "*************************************************".WriteCentered(ConsoleColor.Green);
            "           FINAL MISSION DEBRIEF                ".WriteCentered(ConsoleColor.Green);
            "*************************************************".WriteCentered(ConsoleColor.Green);

            var sortedResults = boardManager.GetSortedLeaderboard(players);
            boardManager.DisplayTable(sortedResults);

            boardManager.HandlePotentialTies(sortedResults, settings);

            "SESSION COMPLETE. LOGGING OUT.".WriteCentered(ConsoleColor.Yellow);
            Thread.Sleep(2000);
        }
    }
}
