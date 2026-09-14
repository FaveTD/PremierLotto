using System;
using System.Collections.Generic;
using System.Text;

namespace PremierLotto.Core
{
    public class GameConfig
    {
        public string TicketType { get; init; }
        public string ValueRange { get; init; }
        public string GameLength { get; init; }
        public string PassTarget { get; init; }
        public string Duplicates { get; init; }
        public string WinMatrix { get; init; }
        public bool IsAlphanumeric { get; init; }
        public int MaxValue { get; init; }
        public bool AllowDuplicates { get; init; }
        public double PassThreshold { get; init; }
        public int NumberOfRounds { get; init; }
    }
}
