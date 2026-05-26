using System;

namespace PremierLotto
{
    public class PayoutCalculator
    {
        public decimal CalculateWinnings(int matches, RiskLevel level, decimal stake)
        {
            decimal multiplier = level switch
            {
                RiskLevel.Easy => 1.5m,   
                RiskLevel.Classic => 3.0m, 
                RiskLevel.Pro => 10.0m,    
                _ => 1.0m
            };

            decimal accuracyPercentage = matches / 4.0m;

            decimal finalPayout = stake * multiplier * accuracyPercentage;

            return Math.Round(finalPayout, 2);
        }
    }
}