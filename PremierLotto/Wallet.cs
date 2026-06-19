using PremierLotto.Utilities;
using System;

namespace PremierLotto.FInance
{
    public class Wallet
    {
        public decimal Balance { get; private set; }
        public decimal DebtOwed { get; private set; } 

        public Wallet()
        {
            Balance = 5000.00m;
            DebtOwed = 0.00m;
        }
        public Wallet(decimal startingAmount, decimal existingDebt = 0.00m)
        {
            Balance = startingAmount;
            DebtOwed = existingDebt;
        }
        public decimal ProcessRoundStake()
        {
            decimal minimumStake = 200.00m;
            decimal baseStake = 0.00m;

            while (true)
            {
                Console.Write($"Enter your stake for this round (Minimum ₦{minimumStake:N0}): ₦");
                if (decimal.TryParse(Console.ReadLine(), out baseStake) && baseStake >= minimumStake)
                {
                    break;
                }
                ($"❌ Invalid stake! You must enter an amount of ₦{minimumStake:N0} or higher.").WriteColored(ConsoleColor.Red);
            }

            decimal debtSurcharge = 0.00m;
            if (DebtOwed > 0)
            {
                debtSurcharge = baseStake * 0.20m; 

                if (debtSurcharge > DebtOwed)
                {
                    debtSurcharge = DebtOwed;
                }
            }

            decimal totalCostForRound = baseStake + debtSurcharge;

            if (Balance >= totalCostForRound)
            {
                Balance -= totalCostForRound;

                if (debtSurcharge > 0)
                {
                    DebtOwed -= debtSurcharge;
                    ($"🏛️ [ENTRY LOAN TAX]: ₦{debtSurcharge:N2} withheld from fee. Remaining Debt: ₦{DebtOwed:N2}").WriteColored(ConsoleColor.DarkYellow);
                }

                ($"✅ Entry Approved! Base Stake: ₦{baseStake:N2} | Total Paid: ₦{totalCostForRound:N2} | Current Balance: ₦{Balance:N2}").WriteColored(ConsoleColor.Green);
                
                return baseStake; 
            }

            ($"\n❌ TRANSACTION DENIED: Total cost is ₦{totalCostForRound:N2}, but your balance is ₦{Balance:N2}.").WriteColored(ConsoleColor.Red);
            
            if (DebtOwed > 0)
            {
                ($"⛔ DEBT LOCKOUT: You have an outstanding house loan of ₦{DebtOwed:N2}.").WriteColored(ConsoleColor.DarkRed);
                ("You cannot request additional credit until this balance is 100% cleared.").WriteColored(ConsoleColor.DarkRed);

                return 0.00m; 
            }
            else
            {
                Console.Write("Your record is clean! Would you like to borrow a flat ₦1,000 from the House to play? (Y/N): ");
                string choice = Console.ReadLine()?.Trim().ToUpper();

                if (choice == "Y")
                {
                    DebtOwed += 1000.00m;
                    Balance += 1000.00m;

                    ($"💰 Loan Approved! ₦1,000 credited to account. Retrying entry prompt...").WriteColored(ConsoleColor.Green);
                    
                    return ProcessRoundStake();
                }
                else
                {
                    Console.WriteLine("Transaction cancelled. Skipping player turn...");
                    return 0.00m;
                }
            }
        }

        public void DepositWinnings(decimal sharedPotAmount)
        {
            if (sharedPotAmount <= 0) return;

            if (DebtOwed > 0)
            {
                decimal winningTax = sharedPotAmount * 0.20m;

                if (winningTax > DebtOwed)
                {
                    winningTax = DebtOwed;
                }

                DebtOwed -= winningTax;
                decimal netWinnings = sharedPotAmount - winningTax;
                Balance += netWinnings;

                ("\n🚨 🏛️ [JACKPOT TAX INTERCEPT]: The House took a 20% cut from your victory pot!").WriteColored(ConsoleColor.Red);
                ($"Withheld from winnings: -₦{winningTax:N2}").WriteColored(ConsoleColor.Red);
                ($"Credited to wallet:      +₦{netWinnings:N2}").WriteColored(ConsoleColor.Red);
                ($"Remaining Debt to House:  ₦{DebtOwed:N2}").WriteColored(ConsoleColor.Red);
            }
            else
            {
                Balance += sharedPotAmount;
                ($"💰 [JACKPOT DEPOSIT]: ₦{sharedPotAmount:N2} credited directly to your wallet balance!").WriteColored(ConsoleColor.Green);
            }
        }
    }
}
