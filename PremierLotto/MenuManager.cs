using PremierLotto.Core;
using PremierLotto.Data;
using System;

namespace PremierLotto.Utilities
{
    public class MenuManager
    {
        public static GameOption ShowMainMenuAndSelect()
        {
            GameOption selectedOption = null;
            HistoryManager historyMenuController = new HistoryManager();

            while (selectedOption == null)
            {
                Console.Clear();
                "***************************************".WriteCentered(ConsoleColor.Yellow);
                "** MAIN OPERATION TERMINAL     **".WriteCentered(ConsoleColor.Yellow);
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
                    selectedOption = GameSettings.ShowMenuAndSelect();
                }
                else
                {
                    "Invalid Selection. Please choose 1 or 2.".WriteColored(ConsoleColor.Red);
                     System.Threading.Thread.Sleep(1000);
                }
            }

            return selectedOption;
        }
    }
}
