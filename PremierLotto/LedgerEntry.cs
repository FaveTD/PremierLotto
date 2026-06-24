using System;

namespace PremierLotto.Models
{
    public class LedgerEntry
    {
        public string EntryId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Type { get; set; } 
        public string Account { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
