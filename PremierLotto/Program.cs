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

            Console.Write("\nEnter your Stake/Investment per round (e.g., 500): ₦");
            if (!decimal.TryParse(Console.ReadLine(), out decimal userStake))
            {
                userStake = 100;
                "Invalid amount. Defaulting to ₦100.".WriteColored(ConsoleColor.Gray);
            }

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
            InputHandler inputSec = new InputHandler();
            MatchCheck checker = new MatchCheck();
            LeaderboardManager boardManager = new LeaderboardManager();
            List<Player> playersList = new List<Player>();
            PayoutCalculator calculator = new PayoutCalculator();

            int totalPlayers;
            while (true)
            {
                Console.Write("\nEnter number of agents participating (2-10): ");
                if (validator.IsValidPlayerCount(Console.ReadLine(), out totalPlayers)) break;
                "⚠️ Protocol Error: Enter a count between 2 and 10.".WriteColored(ConsoleColor.Red);
            }

            for (int i = 1; i <= totalPlayers; i++)
            {
                Console.WriteLine($"\n--- Registering Agent {i} ---");
                Console.Write("Legal Name: ");
                string real = Console.ReadLine();
                Console.Write("Operation Alias: ");
                string alias = Console.ReadLine();
                playersList.Add(new Player(real, alias));
            }

            int totalRounds = settings.NumberOfRounds;

            for (int r = 1; r <= totalRounds; r++)
            {
                Console.Clear();
                "=======================================".WriteCentered(ConsoleColor.Cyan);
                $"       TOURNAMENT ROUND {r} OF {totalRounds}".WriteCentered(ConsoleColor.Cyan);
                "=======================================".WriteCentered(ConsoleColor.Cyan);

                foreach (var player in playersList)
                {
                    Console.WriteLine($"\nAgent [{player.PlayerAlias.ToUpper()}], prepare your guesses.");
                    player.Guesses = inputSec.GetConfirmedGuesses(validator, settings);
                }

                LottoEngine draw = new LottoEngine(settings);
                "SHUFFLING DIGITAL DRUM...".AnimatedWrite(60);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nWINNING COMBINATION: [ {string.Join(" | ", draw.WinningNumbers)} ]");
                Console.ResetColor();

                foreach (var player in playersList)
                {
                    int matches = checker.GetMatchCount(player.Guesses, draw.WinningNumbers);

                    decimal roundWinnings = calculator.CalculateWinnings(matches, settings, userStake);

                    player.CorrectMatches += matches;
                    player.TotalWinnings += roundWinnings;

                    Console.WriteLine($"Agent {player.PlayerAlias}: {matches} Matches | Round Payout: ₦{roundWinnings}");
                }

                if (r < totalRounds)
                {
                    Console.WriteLine("\nRound Complete. Press any key to proceed to next stage...");
                    Console.ReadKey();
                }
            }

            Console.Clear();
            "*************************************************".WriteCentered(ConsoleColor.Green);
            "           FINAL MISSION DEBRIEF                ".WriteCentered(ConsoleColor.Green);
            "*************************************************".WriteCentered(ConsoleColor.Green);

            var sortedResults = boardManager.GetSortedLeaderboard(playersList);
            boardManager.DisplayTable(sortedResults);

            if (settings.HasRollup)
            {
                var tied = boardManager.GetTiedWinners(sortedResults);
                if (tied.Count > 1)
                {
                    "🚨 TIE DETECTED! INITIATING ROLLUP PROTOCOL (SUDDEN DEATH)...".WriteColored(ConsoleColor.Red);
                    Thread.Sleep(2000);
                    boardManager.RunTieBreaker(tied, inputSec, validator, settings, checker);
                }
            }

            "SESSION COMPLETE. LOGGING OUT OF PREMIER LOTTO.".WriteCentered(ConsoleColor.Yellow);
            Thread.Sleep(2000);
        }
    }
}
