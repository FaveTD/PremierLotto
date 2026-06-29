using PremierLotto.Models;
using PremierLotto.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PremierLotto.Data
{
    public class HistoryManager
    {
        private const string HistoryPath = "game_history.json";

        private List<GameLog> LoadAllLogs()
        {
            if (!File.Exists(HistoryPath)) return new List<GameLog>();
            try
            {
                string json = File.ReadAllText(HistoryPath);
                return JsonSerializer.Deserialize<List<GameLog>>(json) ?? new List<GameLog>();
            }
            catch
            {
                return new List<GameLog>(); 
            }
        }

        public void AppendTournamentLog(string modeName, decimal totalPool, List<Player> activePlayers, Dictionary<Player, int> finalScores)
        {
            List<GameLog> totalHistory = LoadAllLogs();

            GameLog currentTournamentLog = new GameLog(modeName, totalPool);

            foreach (var p in activePlayers)
            {
                int roundWins = finalScores.ContainsKey(p) ? finalScores[p] : 0;

                currentTournamentLog.PlayersData.Add(new PlayerRoundRecord(
                    p.PlayerAlias,
                    p.Guesses,
                    roundWins,
                    p.TotalWinnings
                ));
            }

            totalHistory.Add(currentTournamentLog);
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

            if (string.IsNullOrEmpty(targetAlias)) return;

            List<GameLog> allLogs = LoadAllLogs();

            var agentLogs = allLogs.Where(log =>
                log.PlayersData != null && log.PlayersData.Any(p => p.PlayerAlias.Trim().ToLower() == targetAlias)).ToList();

            if (!agentLogs.Any())
            {
                Console.WriteLine("\nNo historic tournament logs found for this agent profile.");
                Console.WriteLine("\nPress any key to return to terminal execution...");
                Console.ReadKey();
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

            Console.WriteLine($"\n--- Found {agentLogs.Count} Tournament Record(s) ---");
            foreach (var log in agentLogs)
            {
                var record = log.PlayersData.First(p => p.PlayerAlias.Trim().ToLower() == targetAlias);
                Console.WriteLine($"[{log.Timestamp:yyyy-MM-dd HH:mm}] ID: {log.GameId} | Mode: {log.GameMode,-7} | Wins: {record.MatchesCount} Rounds | Payout: ₦{record.WinningsClaimed:N2}");
            }

            var personalBestLog = agentLogs
                .Select(log => new { Log = log, Record = log.PlayersData.First(p => p.PlayerAlias.Trim().ToLower() == targetAlias) })
                .OrderByDescending(x => x.Record.WinningsClaimed)
                .ThenByDescending(x => x.Record.MatchesCount)
                .FirstOrDefault();

            if (personalBestLog != null && personalBestLog.Record.WinningsClaimed > 0)
            { 
                ("\n=======================================").WriteColored(ConsoleColor.Yellow);
                ($"⭐ PERSONAL BEST TOURNAMENT RECORD ⭐").WriteColored(ConsoleColor.Yellow);
                ($"Game ID:       {personalBestLog.Log.GameId} ({personalBestLog.Log.Timestamp})").WriteColored(ConsoleColor.Yellow);
                ($"Mode Chosen:   {personalBestLog.Log.GameMode}").WriteColored(ConsoleColor.Yellow);
                ($"Total Wins:    {personalBestLog.Record.MatchesCount} Round(s) Cleared").WriteColored(ConsoleColor.Yellow);
                ($"Prize Payout:  ₦{personalBestLog.Record.WinningsClaimed:N2}").WriteColored(ConsoleColor.Yellow);
                ("=======================================").WriteColored(ConsoleColor.Yellow);
                
            }

            Console.WriteLine("\nPress any key to return to terminal execution...");
            Console.ReadKey();
        }
    }
}
