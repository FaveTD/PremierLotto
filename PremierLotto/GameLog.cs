using System;
using System.Collections.Generic;

namespace PremierLotto
{
    public class PlayerRoundRecord
    {
        public string PlayerAlias { get; set; }
        public List<string> PlayerGuesses { get; set; }
        public int MatchesCount { get; set; }

        public PlayerRoundRecord() { }

        public PlayerRoundRecord(string alias, List<string> guesses, int matches)
        {
            PlayerAlias = alias;
            PlayerGuesses = new List<string>(guesses);
            MatchesCount = matches;
        }
    }

    public class GameLog
    {
        public string GameId { get; set; }
        public DateTime Timestamp { get; set; }
        public string GameMode { get; set; }
        public List<string> WinningNumbers { get; set; }
        public List<PlayerRoundRecord> PlayersData { get; set; }

        public GameLog() { }

        public GameLog(string modeName, List<string> winningNumbers)
        {
            GameId = "Lotto-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            Timestamp = DateTime.Now;
            GameMode = modeName;
            WinningNumbers = new List<string>(winningNumbers);
            PlayersData = new List<PlayerRoundRecord>();
        }
    }
}
