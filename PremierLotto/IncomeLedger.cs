using System;
using System.Collections.Generic;

namespace PremierLotto.Finance
{
    public class IncomeLedger
    {
        private List<decimal> _incomeRecords = new List<decimal>();

        public void RecordTax(decimal taxAmount)
        {
            _incomeRecords.Add(taxAmount);
        }

        public decimal GetTotalIncome() => _incomeRecords.Sum();
    }
}
