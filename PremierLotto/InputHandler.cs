using System;
using System.Text;

namespace PremierLotto
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

                        Console.Write("\b \b*");
                    }
                }
            }
            return sb.ToString();
        }
        public List<string> GetConfirmedGuesses(Validation validator, GameSettings settings)
        {
            while (true)
            {
                Console.WriteLine("\n[SECURE MODE] Type your 4 guesses (visible for 0.5s):");
                string secretInput = GetMaskedInput(); 

                if (validator.TryParseGuesses(secretInput, settings, out List<string> valid))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n✅ Input Verified & Encrypted.");
                    Console.ResetColor();

                    Console.WriteLine("Press ENTER to lock these numbers, or any other key to restart...");

                    if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine("🔒 Locked.");
                        Thread.Sleep(800);
                        Console.Clear(); 
                        return valid; 
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠️ Invalid. Check range, duplicates, or ensure you entered 4 items.");
                    Console.ResetColor();
                    Thread.Sleep(1500);
                    Console.Clear();
                }
            }
        }
    }
}
