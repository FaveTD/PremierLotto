using PremierLotto.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PremierLotto.Core
{
    public class GameOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxNumber { get; set; }
        public int NumberOfRounds { get; set; }
        public bool HasRollup { get; set; }

        public GameOption(int id, string name, int max, int rounds, bool rollup)
        {
            Id = id;
            Name = name;
            MaxNumber = max;
            NumberOfRounds = rounds;
            HasRollup = rollup;
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

        public static List<GameOption> AvailableGames = new List<GameOption>
        {
            new GameOption(1, "Easy", 30, 2, false),
            new GameOption(2, "Classic", 60, 3, true),
            new GameOption(3, "Pro", 90, 5, true)
        };

        public GameSettings(GameOption selectedOption)
        {
            ModeName = selectedOption.Name;
            MaxNumber = selectedOption.MaxNumber;
            NumberOfRounds = selectedOption.NumberOfRounds;
            HasRollup = selectedOption.HasRollup;
            
            if (ModeName == "Easy")
            {
                AllowDuplicates = true;
                IsAlphanumeric = false;
            }
            else if (ModeName == "Classic")
            {
                AllowDuplicates = false;
                IsAlphanumeric = false;
            }
            else if (ModeName == "Pro")
            {
                AllowDuplicates = false;
                IsAlphanumeric = true;
            }

            ShowLevelRequirements();
        }

        private void ShowLevelRequirements()
        {
            Console.Clear();

            "┌────────────────────────────────────────────────────────┐".WriteCentered(ConsoleColor.Cyan);
            "│              ► PREMIER LOTTO SYSTEM CONFIG ◄           │".WriteCentered(ConsoleColor.Cyan);
            "└────────────────────────────────────────────────────────┘".WriteCentered(ConsoleColor.Cyan);

            Console.WriteLine();
            $"MODE SELECTION: » {ModeName.ToUpper()} «".WriteCentered(ConsoleColor.White);
            Console.WriteLine();

            "┌────────────────────────────────────────────────────────┐".WriteCentered(ConsoleColor.DarkGray);

            if (ModeName == "Easy")
            {
                "│  TICKET TYPE   :  Numeric Only                         │".WriteCentered(ConsoleColor.White);
                "│  VALUE RANGE   :  0 to 30                              │".WriteCentered(ConsoleColor.White);
                "│  GAME LENGTH   :  2 Full Rounds                        │".WriteCentered(ConsoleColor.White);
                "│  PASS TARGET   :  25% Match Needed                     │".WriteCentered(ConsoleColor.White);
                "│  DUPLICATES    :  ALLOWED (Shared Pools)               │".WriteCentered(ConsoleColor.White);
                "│  WIN MATRIX    :  MULTIPLE WINNERS ALLOWED             │".WriteCentered(ConsoleColor.White);
            }
            else if (ModeName == "Classic")
            {
                "│  TICKET TYPE   :  Numeric Only                         │".WriteCentered(ConsoleColor.White);
                "│  VALUE RANGE   :  0 to 60                              │".WriteCentered(ConsoleColor.White);
                "│  GAME LENGTH   :  3 Full Rounds                        │".WriteCentered(ConsoleColor.White);
                "│  PASS TARGET   :  50% Match Needed                     │".WriteCentered(ConsoleColor.White);
                "│  DUPLICATES    :  STRICTLY PROHIBITED                  │".WriteCentered(ConsoleColor.White);
                "│  WIN MATRIX    :  SINGLE UNIQUE WINNER                 │".WriteCentered(ConsoleColor.White);
            }
            else if (ModeName == "Pro")
            {
                "│  TICKET TYPE   :  ALPHANUMERIC HARDCODED               │".WriteCentered(ConsoleColor.White);
                "│  VALUE RANGE   :  0 to 90 + Letters A-Z                │".WriteCentered(ConsoleColor.White);
                "│  GAME LENGTH   :  5 Full Rounds                        │".WriteCentered(ConsoleColor.White);
                "│  PASS TARGET   :  75% Match Needed                     │".WriteCentered(ConsoleColor.White);
                "│  DUPLICATES    :  STRICTLY PROHIBITED                  │".WriteCentered(ConsoleColor.White);
                "│  WIN MATRIX    :  SINGLE UNIQUE WINNER                 │".WriteCentered(ConsoleColor.White);
            }

            "└────────────────────────────────────────────────────────┘".WriteCentered(ConsoleColor.DarkGray);

            Console.WriteLine();
            "======================================================".WriteCentered(ConsoleColor.Black);
            "   » Press ANY KEY to load configuration & play...".WriteCentered(ConsoleColor.Green);
            "======================================================".WriteCentered(ConsoleColor.Black);

            Console.ReadKey();
        }

        public static GameOption ShowMenuAndSelect()
        {
            while (true)
            {
                Console.WriteLine("\nSelect Risk Level:");
                foreach (var game in AvailableGames)
                {
                    Console.WriteLine($"{game.Id}. {game.Name} ");
                }

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    var selected = AvailableGames.FirstOrDefault(g => g.Id == choice);
                    if (selected != null) return selected;
                }

                ("Invalid Selection. Try again.").WriteColored(ConsoleColor.Red);
            }
        }
    }
}
