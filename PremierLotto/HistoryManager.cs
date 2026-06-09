using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PremierLotto
{
    public class HistoryManager
    {
        private const string HistoryPath = "game_history.json";

        private List<GameLog> LoadAllLogs()
        {
            if (!File.Exists(HistoryPath)) return new List<GameLog>();
            string json = File.ReadAllText(HistoryPath);
            return JsonSerializer.Deserialize<List<GameLog>>(json) ?? new List<GameLog>();
        }

        public void AppendRoundLog(string modeName, List<string> winningNumbers, List<Player> activePlayers)
        {
            List<GameLog> totalHistory = LoadAllLogs();
            GameLog currentRoundLog = new GameLog(modeName, winningNumbers);

            foreach (var p in activePlayers)
            {
                var playerChecker = new MatchCheck();
                int exactMatches = playerChecker.GetMatchCount(p.Guesses, winningNumbers);

                currentRoundLog.PlayersData.Add(new PlayerRoundRecord(p.PlayerAlias, p.Guesses, exactMatches));
            }

            totalHistory.Add(currentRoundLog);
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(HistoryPath, JsonSerializer.Serialize(totalHistory, options));
        }

        public void LaunchHistoryMenu()
        {
            Console.Clear();
            "=======================================".WriteCentered(ConsoleColor.Cyan);
            "       TERMINAL SYSTEM HISTORY         ".WriteCentered(ConsoleColor.Cyan);
            "=======================================".WriteCentered(ConsoleColor.Cyan);

            Console.Write("\nEnter Agent Alias to search: ");
            string targetAlias = Console.ReadLine()?.Trim().ToLower();

            List<GameLog> allLogs = LoadAllLogs();

            var agentLogs = allLogs.Where(log =>
                log.PlayersData.Any(p => p.PlayerAlias.Trim().ToLower() == targetAlias)).ToList();

            if (!agentLogs.Any())
            {
                Console.WriteLine("\n[SYSTEM]: No historic logs found for this agent profile.");
                return;
            }

            Console.Write("Enter Start Date (YYYY-MM-DD) or press Enter to skip: ");
            string startInput = Console.ReadLine();
            Console.Write("Enter End Date (YYYY-MM-DD) or press Enter to skip: ");
            string endInput = Console.ReadLine();

            if (DateTime.TryParse(startInput, out DateTime start)) 
                agentLogs = agentLogs.Where(l => l.Timestamp.Date >= start.Date).ToList();
            if (DateTime.TryParse(endInput, out DateTime end))
                agentLogs = agentLogs.Where(l => l.Timestamp.Date <= end.Date).ToList();

            Console.WriteLine($"\n--- Found {agentLogs.Count} Game Record(s) ---");
            foreach (var log in agentLogs)
            {
                var record = log.PlayersData.First(p => p.PlayerAlias.Trim().ToLower() == targetAlias);
                Console.WriteLine($"[{log.Timestamp:yyyy-MM-dd HH:mm}] ID: {log.GameId} | Mode: {log.GameMode} | Score: {record.MatchesCount} Matches");
            }

            var personalBestLog = agentLogs
                .Select(log => new { Log = log, Record = log.PlayersData.First(p => p.PlayerAlias.Trim().ToLower() == targetAlias) })
                .OrderByDescending(x => x.Record.MatchesCount)
                .ThenByDescending(x => x.Log.Timestamp)
                .FirstOrDefault();

            if (personalBestLog != null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n=======================================");
                Console.WriteLine($"⭐ PERSONAL BEST ROUND REPLAY ⭐");
                Console.WriteLine($"Game ID: {personalBestLog.Log.GameId} ({personalBestLog.Log.Timestamp})");
                Console.WriteLine($"Mode:    {personalBestLog.Log.GameMode}");
                Console.WriteLine($"Winning Combo: [ {string.Join(" | ", personalBestLog.Log.WinningNumbers)} ]");
                Console.WriteLine($"Your Guesses:  [ {string.Join(" | ", personalBestLog.Record.PlayerGuesses)} ]");
                Console.WriteLine($"Final Score:   {personalBestLog.Record.MatchesCount} Matches");
                Console.WriteLine("=======================================");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to return to terminal execution...");
            Console.ReadKey();
        }
    }
}
