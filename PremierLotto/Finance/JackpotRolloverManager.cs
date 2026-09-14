using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;

namespace PremierLotto.Finance
{
    public class JackpotRolloverManager
    {
        private const string FilePath = "jackpot_rollover.json";
        private Dictionary<string, decimal> _rollovers;
        public JackpotRolloverManager()
        {
            LoadRollovers();
        }

        private void LoadRollovers()
        {
            if (!File.Exists(FilePath))
            {
                _rollovers = new Dictionary<string, decimal>();
                return;
            }
            string jsonString = File.ReadAllText(FilePath);
            _rollovers = JsonSerializer.Deserialize<Dictionary<string, decimal>>(jsonString)
                         ?? new Dictionary<string, decimal>();
         }

        private void SaveRollovers()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_rollovers, options);
            File.WriteAllText(FilePath, json);
        }

        public decimal ClaimCarriedAmount(string modeName)
        {
            if(_rollovers.TryGetValue(modeName, out decimal carried) && carried > 0)
            {
                _rollovers[modeName] = 0;
                SaveRollovers();
                return carried;
            }

            return 0;
        }

        public void AddToRollover(string modeName, decimal amount)
        {
            if (amount <= 0) return;
            if (_rollovers.ContainsKey(modeName))
                _rollovers[modeName] += amount;
            else
                _rollovers[modeName] = amount;
            SaveRollovers();
        }

        public decimal GetCurrentRollover(string modeName)
        {
            return _rollovers.TryGetValue(modeName, out decimal amount) ? amount : 0;
        }
    }
}
