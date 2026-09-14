using PremierLotto.Finance;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace PremierLotto.Game
{
    public class PoolManager
    {
        private readonly FinanceManager _finance;
        private decimal _totalPool = 0;
        private readonly string _gameMode;
        private const string JackpotFilePath = "jackpot_config.json";

        public PoolManager(FinanceManager finance, string gameMode)
        {
            _finance = finance;
            _gameMode = gameMode;
            LoadJackpots();
        }

        private void LoadJackpots()
        {
            if (File.Exists(JackpotFilePath))
            {
                string json = File.ReadAllText(JackpotFilePath);
                var allJackpots = JsonSerializer.Deserialize<Dictionary<string, decimal>>(json);
                if (allJackpots != null && allJackpots.ContainsKey(_gameMode))
                {
                    _totalPool = allJackpots[_gameMode];
                }
            }
        }

        public void SaveJackpot()
        {
            var allJackpots = new Dictionary<string, decimal>();
            if (File.Exists(JackpotFilePath))
            {
                string json = File.ReadAllText(JackpotFilePath);
                allJackpots = JsonSerializer.Deserialize<Dictionary<string, decimal>>(json) ?? allJackpots;
            }

            allJackpots[_gameMode] = _totalPool;
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(JackpotFilePath, JsonSerializer.Serialize(allJackpots, options));
        }

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

        public void ResetPool()
        {
            _totalPool = 0;
            SaveJackpot();
        }

        public void AddExternalAmount(decimal amount) => _totalPool += amount;
    }
}