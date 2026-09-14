using System;
using PremierLotto.Utilities;

namespace PremierLotto.Core
{
    public enum GameLevel { Easy, Classic, Pro }

    public class GameSettings
    {
        public GameLevel Level { get; private set; }
        public GameConfig Config { get; private set; }

        public static GameLevel ShowMenuAndSelect()
        {
            while (true)
            {
                Console.WriteLine("\nSelect Risk Level:");
                foreach (GameLevel level in Enum.GetValues(typeof(GameLevel)))
                {
                    Console.WriteLine($"{(int)level + 1}. {level}");
                }

                if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= Enum.GetNames(typeof(GameLevel)).Length)
                {
                    return (GameLevel)(choice - 1);
                }

                ("Invalid Selection. Try again.").WriteColored(ConsoleColor.Red);
            }
        }

        public GameSettings(GameLevel level)
        {
            Level = level;
            Config = GameData.Levels[level];
            ShowLevelRequirements();
        }

        private void ShowLevelRequirements()
        {
            Console.Clear();
            "┌────────────────────────────────────────────────────────┐".WriteCentered(ConsoleColor.Cyan);
            "│              ► PREMIER LOTTO SYSTEM CONFIG ◄           │".WriteCentered(ConsoleColor.Cyan);
            "└────────────────────────────────────────────────────────┘".WriteCentered(ConsoleColor.Cyan);

            Console.WriteLine();
            $"MODE SELECTION: » {Level.ToString().ToUpper()} «".WriteCentered(ConsoleColor.White);
            Console.WriteLine();

            "┌────────────────────────────────────────────────────────┐".WriteCentered(ConsoleColor.DarkGray);
            $"│  TICKET TYPE   :  {Config.TicketType,-25}  │".WriteCentered(ConsoleColor.White);
            $"│  VALUE RANGE   :  {Config.ValueRange,-25}  │".WriteCentered(ConsoleColor.White);
            $"│  GAME LENGTH   :  {Config.GameLength,-25}  │".WriteCentered(ConsoleColor.White);
            $"│  PASS TARGET   :  {Config.PassTarget,-25}  │".WriteCentered(ConsoleColor.White);
            $"│  DUPLICATES    :  {Config.Duplicates,-25}  │".WriteCentered(ConsoleColor.White);
            $"│  WIN MATRIX    :  {Config.WinMatrix,-25}    │".WriteCentered(ConsoleColor.White);
            "└────────────────────────────────────────────────────────┘".WriteCentered(ConsoleColor.DarkGray);

            Console.WriteLine();
            "======================================================".WriteCentered(ConsoleColor.Black);
            "   » Press ANY KEY to load configuration & play...".WriteCentered(ConsoleColor.Green);
            "======================================================".WriteCentered(ConsoleColor.Black);
            Console.ReadKey();
        }
    }
}