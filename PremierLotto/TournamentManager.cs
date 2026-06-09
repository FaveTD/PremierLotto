using System;
using System.Collections.Generic;
using PremierLotto;

public static class TournamentManager
{
    public static void ProcessPlayerResults(List<Player> players, List<string> winningNumbers, GameSettings settings, decimal stake)
    {
        var checker = new MatchCheck();
        var calculator = new PayoutCalculator();

        int highestMatchesThisRound = -1;

        foreach (var p in players)
        {
            p.IsWinnerOfRound = false;
        }

        foreach (var player in players)
        {
            int matches = checker.GetMatchCount(player.Guesses, winningNumbers);
            decimal roundWinnings = calculator.CalculateWinnings(matches, settings, stake);

            player.CorrectMatches += matches;
            player.TotalWinnings += roundWinnings;

            if (matches > highestMatchesThisRound)
            {
                highestMatchesThisRound = matches;
            }

            Console.WriteLine($"Agent {player.PlayerAlias}: {matches} Matches | Round Payout: ₦{roundWinnings}");
        }

        foreach (var player in players)
        {
            if (player.CorrectMatches == highestMatchesThisRound && highestMatchesThisRound > 0)
            {
                player.IsWinnerOfRound = true;
            }
        }
    }

    public static void RunTournament(List<Player> players, GameSettings settings, decimal stake)
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

            ProcessPlayerResults(players, draw.WinningNumbers, settings, stake);

            HistoryManager historyLogger = new HistoryManager();
            historyLogger.AppendRoundLog(settings.ModeName, draw.WinningNumbers, players);

            if (r < settings.NumberOfRounds)
            {
                Console.WriteLine("\nRound Complete. Press any key to proceed...");
                Console.ReadKey();
            }
        }
    }
}
