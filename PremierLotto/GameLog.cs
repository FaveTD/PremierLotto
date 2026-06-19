using PremierLotto.Models;
using System;
using System.Collections.Generic;

namespace PremierLotto.Data
{
    public class GameLog
    {
        public string GameId { get; set; }
        public DateTime Timestamp { get; set; }
        public string GameMode { get; set; }
        public decimal TotalPool { get; set; }
        public List<PlayerRoundRecord> PlayersData { get; set; }

        public GameLog() { }

        public GameLog(string modeName, decimal totalPool)
        {
            GameId = "Lotto-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            Timestamp = DateTime.Now;
            GameMode = modeName;
            TotalPool = totalPool;
            PlayersData = new List<PlayerRoundRecord>();
        }
    }
}
