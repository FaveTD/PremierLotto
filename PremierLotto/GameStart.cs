using System;
using System.Collections.Generic;

namespace PremierLotto
{
    public static class GameStart
    {
        public static decimal GetUserStake()
        {
            Console.Write("\nEnter your Stake/Investment per round (e.g., 500): ₦");
            if (!decimal.TryParse(Console.ReadLine(), out decimal stake))
            {
                "Invalid amount. Defaulting to ₦100.".WriteColored(ConsoleColor.Gray);
                return 100;
            }
            return stake;
        }

        public static void RegisterAgents(Validation validator, List<Player> playersList)
        {
            int totalPlayers;
            while (true)
            {
                Console.Write("\nEnter number of agents participating (2-10): ");
                if (validator.IsValidPlayerCount(Console.ReadLine(), out totalPlayers)) break;
                "⚠️ Protocol Error: Enter a count between 2 and 10.".WriteColored(ConsoleColor.Red);
            }

            for (int i = 1; i <= totalPlayers; i++)
            {
                Console.WriteLine($"\n--- Registering Agent {i} ---");
                Console.Write("Legal Name: ");
                string real = Console.ReadLine();
                Console.Write("Operation Alias: ");
                string alias = Console.ReadLine();
                playersList.Add(new Player(real, alias));
            }
        }
        public static bool VerifyAgentAccess()
        {
            Console.WriteLine("\nAgent, enter your age to access the terminal:");
    
            int age;
            if (!Console.ReadLine().IsValidAge(out age)) 
            {
                return false; 
            }

            "Access Granted. Initializing System...".AnimatedWrite(40);
             Thread.Sleep(1000);
             return true; 
        }
    }
}
