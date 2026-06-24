using System;
using System.Collections.Generic;
using System.IO;                 
using System.Text.Json;          
using PremierLotto.Models;       
using PremierLotto.Finance;

namespace PremierLotto.Finance
{
    public class FinanceManager
    {
        public FundingLedger Funding { get; } = new FundingLedger();
        public IncomeLedger Income { get; } = new IncomeLedger();

        public decimal CalculateTax(decimal stakeAmount) => stakeAmount * 0.10m;

        public void RegisterTaxFromStake(decimal stakeAmount)
        {
            decimal tax = CalculateTax(stakeAmount);
            Income.RecordTax(tax);
        }

        public void LogTransaction(string type, string account, decimal amount, string description)
        {
            string filePath = "system_ledger.json";
            List<LedgerEntry> ledger = new List<LedgerEntry>();

            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    ledger = JsonSerializer.Deserialize<List<LedgerEntry>>(json) ?? new List<LedgerEntry>();
                }
                catch
                {
                    ledger = new List<LedgerEntry>();
                }
            }

            ledger.Add(new LedgerEntry
            {
                EntryId = Guid.NewGuid().ToString().Substring(0, 8),
                Timestamp = DateTime.Now,
                Type = type,
                Account = account,
                Amount = amount,
                Description = description
            });

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(filePath, JsonSerializer.Serialize(ledger, options));
        }
    }
}
