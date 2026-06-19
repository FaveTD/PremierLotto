using PremierLotto.Data;
using PremierLotto.Models;
using PremierLotto.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PremierLotto.Core
{
    public static class GameStart
    {
        public static void RegisterAgents(Validation validator, List<Player> playersList, ProfileDataManager dataManager)
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
                Console.Clear();
                "***************************************".WriteCentered(ConsoleColor.Yellow);
                "** AGENT TERMINAL LOGIN              **".WriteCentered(ConsoleColor.Yellow);
                "***************************************".WriteCentered(ConsoleColor.Yellow);

                Console.WriteLine($"\n--- Registering Agent {i} of {totalPlayers} ---");

                Console.Write("Operation Alias: ");
                string alias = Console.ReadLine();

                PlayerProfile profile = dataManager.GetOrCreateProfile(alias);

                if (string.IsNullOrEmpty(profile.LegalName))
                {
                    Console.Write("New Agent Profile Detected! Enter your Legal Name: ");
                    string realName = Console.ReadLine();
                    profile.LegalName = realName;
                    ($"\n✨ Fresh Profile Created for Agent {profile.DisplayName}!").WriteColored(ConsoleColor.Green);
                }
                else
                {
                    ($"\n👋 Welcome Back, Agent {profile.DisplayName}!").WriteColored(ConsoleColor.Cyan);
                    $"Authenticated Identity: {profile.LegalName}".WriteColored(ConsoleColor.DarkGray);
                }

                playersList.Add(new Player(profile.LegalName, profile.DisplayName, profile.Wallet));

                $"💰 Wallet Loaded! Current Balance: ₦{profile.Wallet.Balance:N2} | Outstanding Debt: ₦{profile.Wallet.DebtOwed:N2}".WriteColored(ConsoleColor.DarkGreen);

                
                if (i < totalPlayers)
                {
                    Console.WriteLine("\n---------------------------------------");
                    (">> Registration complete. Press ENTER to pass terminal to Agent ").WriteColored(ConsoleColor.Yellow);
                    Console.Write((i + 1).ToString());
                    ("...").WriteColored(ConsoleColor.Yellow);
                    Console.ReadLine(); 
                }
                else
                {
                    Console.WriteLine("\n---------------------------------------");
                    (">> All agents registered. Press ENTER to proceed to stake initialization...").WriteColored(ConsoleColor.Yellow);
                    Console.ReadLine(); 
                }
            }
        }

        public static bool VerifyAgentAccess()
        {
            Console.WriteLine("\nAgent, enter your age to access the terminal:");
            int age;
            if (!Console.ReadLine().IsValidAge(out age)) return false;

            "Access Granted. Initializing System...".AnimatedWrite(40);
            Thread.Sleep(1000);
            return true;
        }
    }
}
