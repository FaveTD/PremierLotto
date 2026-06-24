using System.Collections.Generic;
using System.Linq;

namespace PremierLotto.Finance
{
    public class FundingLedger
    {
        private List<decimal> _grantRecords = new List<decimal>();

        public void RecordInitialGrant(decimal amount)
        {
            _grantRecords.Add(-amount);
        }

        public decimal GetTotalSystemDebt()
        {
            return _grantRecords.Sum();
        }
    }
}
