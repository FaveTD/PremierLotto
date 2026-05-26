using System;
using System.Collections.Generic;

namespace PremierLotto
{
    public enum RiskLevel
    {
        Easy = 1,
        Classic = 2,
        Pro = 3
    }

    public class GameSettings
    {
        public int MaxNumber { get; private set; }
        public int NumberOfRounds { get; private set; }
        public bool AllowDuplicates { get; private set; }
        public bool HasRollup { get; private set; }
        public bool IsAlphanumeric { get; private set; }
        public RiskLevel Level { get; private set; }

        public GameSettings(RiskLevel level, bool useDuplicates, bool useAlphanumeric)
        {
            Level = level;
            AllowDuplicates = useDuplicates;
            IsAlphanumeric = useAlphanumeric;

            switch (level)
            {
                case RiskLevel.Easy:
                    MaxNumber = 30;
                    NumberOfRounds = 5;
                    HasRollup = false;
                    break;

                case RiskLevel.Classic:
                    MaxNumber = 60;
                    NumberOfRounds = 3;
                    HasRollup = true;
                    break;

                case RiskLevel.Pro:
                    MaxNumber = 90;
                    NumberOfRounds = 1;
                    HasRollup = true;
                    break;
            }

           
            string duplicateStatus = AllowDuplicates ? "Duplicates Enabled" : "Unique Numbers Only";
            string typeStatus = IsAlphanumeric ? "Alphanumeric" : "Numeric Only";

            Console.WriteLine($"\n--- Configuration: {Level} Mode ---");
            Console.WriteLine($"[{typeStatus}] | [{duplicateStatus}] | [{NumberOfRounds} Rounds]");
        }
    }
}