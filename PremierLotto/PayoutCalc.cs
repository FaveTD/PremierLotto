using System;

namespace PremierLotto
{
    public class PayoutCalculator
    {
        public decimal CalculateWinnings(int matches, GameSettings settings, decimal stake)
        {
            decimal multiplier = settings.Multiplier;

            decimal accuracyPercentage = matches / 4.0m;

            decimal finalPayout = stake * multiplier * accuracyPercentage;

            return Math.Round(finalPayout, 2);
        }
    }
}
