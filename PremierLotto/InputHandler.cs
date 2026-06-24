using PremierLotto.Core;
using PremierLotto.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PremierLotto.Utilities
{
    public class InputHandler
    {
        public decimal GetValidatedStake(Player player)
        {
            while (true)
            {
                Console.Write($"Agent {player.PlayerAlias}, enter stake with a minimum of 200 and a maximum of {player.Wallet.Balance:N2}): ");
                string input = Console.ReadLine();

                if (decimal.TryParse(input, out decimal amount))
                {
                    if (amount < 200)
                        Console.WriteLine("⚠️ Stake too low. Minimum entry is ₦200.");
                    else if (amount > player.Wallet.Balance)
                        Console.WriteLine($"⚠️ Insufficient funds. You only have ₦{player.Wallet.Balance:N2}.");
                    else
                        return amount;
                }
                else
                {
                    Console.WriteLine("⚠️ Invalid input. Please enter a valid number.");
                }
            }
        }

        public string GetMaskedInput()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) { Console.WriteLine(); break; }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    sb.Append(key.KeyChar);
                    Console.Write(key.KeyChar == ' ' ? " " : "*");
                }
            }
            return sb.ToString();
        }

        public List<string> GetConfirmedGuesses(Validation validator, GameSettings settings)
        {
            while (true)
            {
                Console.WriteLine("\nType your 4 guesses separated by spaces:");
                string secretInput = GetMaskedInput();

                if (validator.TryParseGuesses(secretInput, settings, out List<string> valid))
                {
                    ("\n✅ Input Verified & Encrypted.").WriteColored(ConsoleColor.Green);
                    Console.WriteLine("Press ENTER to lock these numbers, or any other key to restart entry...");
                    if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine("🔒 Locked.");
                        Thread.Sleep(800);
                        return valid;
                    }
                }
                else
                {
                    ("⚠️ Invalid. Check entry rules, duplicate limitations, or number bounds.").WriteColored(ConsoleColor.Red);
                    Thread.Sleep(1500);
                    Console.Clear();
                    "RE-ENTERING GUESSES".WriteCentered(ConsoleColor.DarkYellow);
                }
            }
        }
    }
}
