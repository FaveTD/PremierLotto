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
            var config = settings.Config;

            while (WinningNumbers.Count < 4)
            {
                string ball = GenerateBall(random, config);

                if (config.AllowDuplicates || !WinningNumbers.Contains(ball))
                {
                    WinningNumbers.Add(ball);
                }
            }
        }

        private string GenerateBall(Random random, GameConfig config)
        {
            if (config.IsAlphanumeric)
            {
                char letter = (char)random.Next('A', 'Z' + 1);
                int number = random.Next(0, config.MaxValue + 1);
                return $"{letter}{number}";
            }

            return random.Next(0, config.MaxValue + 1).ToString();
        }
    }
}