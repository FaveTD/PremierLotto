using System;
using System.Threading;

namespace PremierLotto
{
    public static class ConsoleExtensions
    {
        public static void WriteCentered(this string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            int windowWidth = Console.WindowWidth;
            int leftPadding = (windowWidth / 2) + (text.Length / 2);
            Console.WriteLine(text.PadLeft(leftPadding));
            Console.ResetColor();
        }
        public static void WriteColored(this string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void AnimatedWrite(this string message, int delayMilliseconds)
        {
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(delayMilliseconds);
            }
            Console.WriteLine();
        }
    }
}