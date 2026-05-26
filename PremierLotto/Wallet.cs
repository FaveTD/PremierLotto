using System;
using System.Threading;

namespace PremierLotto
{
    public class Wallet
    {
        public decimal Balance { get; private set; }
        public Wallet(decimal startingAmount)
        {
            Balance = startingAmount;
        }

        public decimal ProcessTicketPurchase()
        {
            int ticketCost = 1000;
            int minimumStake = 100;

            while (true)
            {
                Console.WriteLine($"Your current wallet balance is: ₦{Balance}");
                Thread.Sleep(1000);

                Console.WriteLine($"Each ticket costs ₦{ticketCost}. How many tickets would you like to purchase?");
                int numberOfTickets = int.Parse(Console.ReadLine());
                int baseCost = numberOfTickets * ticketCost;

                Console.WriteLine($"How much would you like to stake per ticket? (Minimum stake is ₦{minimumStake})");
                int stake = 0;

                while (true)
                {
                    stake = int.Parse(Console.ReadLine());
                    if (stake < minimumStake)
                    {
                        Console.WriteLine($"Stake must be at least ₦{minimumStake}. Please enter a valid stake amount.");
                    }
                    else
                    {
                        break;
                    }
                }

                int totalStake = stake * numberOfTickets;
                decimal grandTotal = baseCost + totalStake;

                if (grandTotal > Balance)
                {
                    Console.WriteLine($"Insufficient funds. Your total cost is ₦{grandTotal} but your wallet balance is only ₦{Balance}. Please adjust your ticket quantity or stake amount.");
                }
                else
                {
                    Balance -= grandTotal;
                    Console.WriteLine($"Payment Successful! Remaining balance: ₦{Balance}");

                    DateTime ticketTime = DateTime.Now;
                    Console.WriteLine($"Receipt Generated at: {ticketTime.ToString("yyyy-MM-dd HH:mm:ss")}");

                    return grandTotal;
                }
            }
        }
        public  decimal Deposit(decimal amount)
        {
            Balance += amount;
            return Balance;
        }
    }
}