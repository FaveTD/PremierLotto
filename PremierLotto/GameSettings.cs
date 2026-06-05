using System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PremierLotto
{
    public class GameOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxNumber { get; set; }
        public int NumberOfRounds { get; set; }
        public bool HasRollup { get; set; }
        public decimal Multiplier { get; set; }

        public GameOption(int id, string name, int max, int rounds, bool rollup, decimal mult)
        {
            Id = id;
            Name = name;
            MaxNumber = max;
            NumberOfRounds = rounds;
            HasRollup = rollup;
            Multiplier = mult;
        }
    }

    public class GameSettings
    {
        public int MaxNumber { get; private set; }
        public int NumberOfRounds { get; private set; }
        public bool AllowDuplicates { get; private set; }
        public bool HasRollup { get; private set; }
        public bool IsAlphanumeric { get; private set; }
        public string ModeName { get; private set; }
        public decimal Multiplier { get; private set; } 

        public static List<GameOption> AvailableGames = new List<GameOption>
        {
            new GameOption(1, "Easy", 30, 5, false, 1.5m),
            new GameOption(2, "Classic", 60, 3, true, 3.0m),
            new GameOption(3, "Pro", 90, 1, true, 10.0m)
        };

        public GameSettings(GameOption selectedOption, bool useDuplicates, bool useAlphanumeric)
        {
            ModeName = selectedOption.Name;
            MaxNumber = selectedOption.MaxNumber;
            NumberOfRounds = selectedOption.NumberOfRounds;
            HasRollup = selectedOption.HasRollup;
            Multiplier = selectedOption.Multiplier;

            AllowDuplicates = useDuplicates;
            IsAlphanumeric = useAlphanumeric;

            string duplicateStatus = AllowDuplicates ? "Duplicates Enabled" : "Unique Numbers Only";
            string typeStatus = IsAlphanumeric ? "Alphanumeric" : "Numeric Only";

            Console.WriteLine($"\n--- Configuration: {ModeName} Mode ---");
            Console.WriteLine($"[{typeStatus}] | [{duplicateStatus}] | [{NumberOfRounds} Rounds] | [{Multiplier}x Multiplier]");
        }

        public static GameOption ShowMenuAndSelect()
        {
            while (true)
            {
                Console.WriteLine("\nSelect Risk Level:");
                foreach (var game in AvailableGames)
                {
                    Console.WriteLine($"{game.Id}. {game.Name} ({game.Multiplier}x Multiplier)");
                }

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    var selected = AvailableGames.FirstOrDefault(g => g.Id == choice);
                    if (selected != null) return selected;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Selection. Try again.");
                Console.ResetColor();
            }
        }
        public static bool GetDuplicatePreference()
        {
            Console.Write("\nEnable Duplicate Numbers in Guesses? (Y/N): ");
            return Console.ReadLine()?.ToUpper() == "Y";
        }
        public static bool GetAlphanumericPreference(GameOption selectedOption)
        {
            bool allowAlpha = false;

            if (selectedOption.Name == "Pro")
            {
                Console.Write("Activate Alphanumeric Mode (A1, B2...)? (Y/N): ");
                allowAlpha = Console.ReadLine()?.ToUpper() == "Y";
            }

            return allowAlpha;
        }
    }

}


