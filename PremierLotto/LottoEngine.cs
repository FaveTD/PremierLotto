using PremierLotto.Core;
using System;
using System.Collections.Generic;

namespace PremierLotto.Game
{
    public class LottoEngine
    {
        public List<string> WinningNumbers { get; private set; }

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
        }
    }
}
