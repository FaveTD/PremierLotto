using PremierLotto.Finance;

namespace PremierLotto.Game
{
    public class PoolManager
    {
        private readonly FinanceManager _finance;
        private decimal _totalPool = 0;

        public PoolManager(FinanceManager finance) => _finance = finance;

        public bool ProcessStake(decimal stakeAmount, Wallet playerWallet)
        {
            if (playerWallet.TryDeductFunds(stakeAmount))
            {
                decimal houseFee = stakeAmount * 0.10m; 
                _totalPool += (stakeAmount - houseFee);

                return true;
            }
            return false;
        }

        public decimal GetCurrentPool() => _totalPool;

        public void ResetPool() => _totalPool = 0;
    }
}
