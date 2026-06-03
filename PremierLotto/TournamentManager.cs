using PremierLotto;

public static class TournamentManager
{
    public static void ProcessPlayerResults(List<Player> players, List<string> winningNumbers, GameSettings settings, decimal stake)
    {
        var checker = new MatchCheck();
        var calculator = new PayoutCalculator();

        foreach (var player in players)
        {
            int matches = checker.GetMatchCount(player.Guesses, winningNumbers);
            decimal roundWinnings = calculator.CalculateWinnings(matches, settings, stake);

            player.CorrectMatches += matches;
            player.TotalWinnings += roundWinnings;

            Console.WriteLine($"Agent {player.PlayerAlias}: {matches} Matches | Round Payout: ₦{roundWinnings}");
        }
    }
}
