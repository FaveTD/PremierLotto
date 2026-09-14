using PremierLotto.Data;
using PremierLotto.Utilities;
using System;

namespace PremierLotto.Core
{
    public class MenuManager
    {
        public static GameLevel? ShowMainMenuAndSelect()
        {
            HistoryManager historyMenuController = new HistoryManager();

            while (true)
            {
                Console.Clear();
                "***************************************".WriteCentered(ConsoleColor.Yellow);
                "**      MAIN OPERATION TERMINAL      **".WriteCentered(ConsoleColor.Yellow);
                "***************************************".WriteCentered(ConsoleColor.Yellow);
                Console.WriteLine("\nSelect an Operation:");
                Console.WriteLine("1. Play Game");
                Console.WriteLine("2. View Past Game History Records");
                Console.Write("\nSelection (1-2): ");

                string systemChoice = Console.ReadLine();

                if (systemChoice == "2")
                {
                    historyMenuController.LaunchHistoryMenu();
                }
                else if (systemChoice == "1")
                {
                    return GameSettings.ShowMenuAndSelect();
                }
                else
                {
                    "Invalid Selection. Please choose 1 or 2.".WriteColored(ConsoleColor.Red);
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }
        public static bool PromptForNextAction()
        {
            Console.Clear();
            ("\nTournament Session Complete.").WriteCentered(ConsoleColor.Green);
            Console.WriteLine("Press [Y] to stay on the app or [N] to exit.");

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                char keyChar = char.ToLower(keyInfo.KeyChar);

                if (keyChar == 'y')
                {
                    return true;
                }
                else if (keyChar == 'n')
                {
                    Console.WriteLine("Exiting application...");
                    return false;
                }
            }
        }
    }
}