using System;
using System.Text.Json.Serialization;

namespace PremierLotto.Finance
{
    public class Wallet
    {
        public decimal Balance { get; set; }
        public decimal DebtOwed { get; set; }

        public Wallet()
        {
            Balance = 5000.00m;
            DebtOwed = 0.00m;
        }

        public Wallet(decimal startingAmount)
        {
            Balance = startingAmount;
            DebtOwed = 0.00m;
        }

        public decimal GetEntryFee() => 1000.00m;

        public bool TryDeductFunds(decimal amount)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
                return true;
            }
            return false;
        }

        public void AddFunds(decimal amount) => Balance += amount;
        public void RecordDebt(decimal amount) => DebtOwed += amount;
        public void PayDownDebt(decimal amount) => DebtOwed -= Math.Min(amount, DebtOwed);

        public void DepositWinnings(decimal amount)
        {
            Balance += amount;
            Console.WriteLine($"[System] ₦{amount:N2} has been deposited into your wallet.");
        }
    }
}
