using System;
using System.Collections.Generic;
using System.Linq;

namespace PremierLotto
{
    public class LottoEngine
    {
        public List<string> WinningNumbers { get; private set; }
        public string GoldenBall { get; private set; }

        public LottoEngine(GameSettings settings)
        {
            Random random = new Random();
            WinningNumbers = new List<string>();

            while (WinningNumbers.Count < 4)
            {
                string ball = "";

                if (settings.IsAlphanumeric)
                {
                    char letter = (char)random.Next('A', 'Z' + 1);
                    int number = random.Next(0, settings.MaxNumber + 1);
                    ball = $"{letter}{number}";
                }
                else
                {
                    ball = random.Next(0, settings.MaxNumber + 1).ToString();
                }

                if (!WinningNumbers.Contains(ball))
                {
                    WinningNumbers.Add(ball);
                }
            }

            while (true)
            {
                string potentialGolden;
                if (settings.IsAlphanumeric)
                {
                    char letter = (char)random.Next('A', 'Z' + 1);
                    potentialGolden = $"{letter}{random.Next(0, settings.MaxNumber + 1)}";
                }
                else
                {
                    potentialGolden = random.Next(0, settings.MaxNumber + 1).ToString();
                }

                if (!WinningNumbers.Contains(potentialGolden))
                {
                    GoldenBall = potentialGolden;
                    break;
                }
            }
        }
    }
}