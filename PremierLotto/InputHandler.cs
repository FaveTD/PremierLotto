using PremierLotto.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PremierLotto.Utilities
{
    public class InputHandler
    {
        public string GetMaskedInput()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        char lastChar = sb[sb.Length - 1];
                        sb.Remove(sb.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    char c = key.KeyChar;
                    sb.Append(c);

                    if (c == ' ')
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write(c);
                        Thread.Sleep(500); 
                        Console.Write("\b*"); 
                    }
                }
            }
            return sb.ToString();
        }

        public List<string> GetConfirmedGuesses(Validation validator, GameSettings settings)
        {
            while (true)
            {
                Console.WriteLine("\nType your 4 guesses separated by spaces (visible for 0.5s):");
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

                    "RE-ENTERING MATRIX PARAMS".WriteCentered(ConsoleColor.DarkYellow);
                }
            }
        }
    }
}
