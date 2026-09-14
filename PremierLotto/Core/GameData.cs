using System;
using System.Collections.Generic;
using System.Text;

namespace PremierLotto.Core
{
    public static class GameData
    {
        public static readonly Dictionary<GameLevel, GameConfig> Levels = new()
    {
        { GameLevel.Easy, new GameConfig {
            TicketType = "Numeric Only", ValueRange = "0 to 30", GameLength = "2 Full Rounds",
            PassTarget = "25% Match Needed", Duplicates = "ALLOWED (Shared Pools)", WinMatrix = "MULTIPLE WINNERS",
            IsAlphanumeric = false, MaxValue = 30, AllowDuplicates = true, PassThreshold = 0.25, NumberOfRounds = 2 }
        },
        { GameLevel.Classic, new GameConfig {
            TicketType = "Numeric Only", ValueRange = "0 to 60", GameLength = "3 Full Rounds",
            PassTarget = "50% Match Needed", Duplicates = "STRICTLY PROHIBITED", WinMatrix = "SINGLE UNIQUE WINNER",
            IsAlphanumeric = false, MaxValue = 60, AllowDuplicates = false, PassThreshold = 0.50, NumberOfRounds = 3  }
        },
        { GameLevel.Pro, new GameConfig {
            TicketType = "ALPHANUMERIC HARDCODED", ValueRange = "0 to 90 + Letters A-Z", GameLength = "5 Full Rounds",
            PassTarget = "75% Match Needed", Duplicates = "STRICTLY PROHIBITED", WinMatrix = "SINGLE UNIQUE WINNER",
            IsAlphanumeric = true, MaxValue = 90, AllowDuplicates = false, PassThreshold = 0.75, NumberOfRounds = 5 }
        }
    };
    }
}
