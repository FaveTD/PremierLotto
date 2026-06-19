using PremierLotto.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PremierLotto.Data
{
    public class ProfileDataManager
    {
        private const string FilePath = "player_profiles.json";

        public Dictionary<string, PlayerProfile> Database { get; private set; }

        public ProfileDataManager()
        {
            LoadProfiles();
        }

        private void LoadProfiles()
        {
            if (!File.Exists(FilePath))
            {
                Database = new Dictionary<string, PlayerProfile>();
                return;
            }

            string jsonString = File.ReadAllText(FilePath);
            Database = JsonSerializer.Deserialize<Dictionary<string, PlayerProfile>>(jsonString)
                       ?? new Dictionary<string, PlayerProfile>();
        }

        public PlayerProfile GetOrCreateProfile(string rawName)
        {
            string key = rawName.Trim().ToLower();

            if (Database.ContainsKey(key))
            {
                PlayerProfile existing = Database[key];


                return existing;
            }

            PlayerProfile newProfile = new PlayerProfile(rawName);
            Database[key] = newProfile;


            return newProfile;
        }

        public void UpdateAndSave(List<Player> playersList)
        {
            foreach (var player in playersList)
            {
                string key = player.PlayerAlias.Trim().ToLower();

                if (Database.ContainsKey(key))
                {
                    Database[key].UpdateStats(player.CorrectMatches, player.IsWinnerOfRound);
                }
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            string updatedJson = JsonSerializer.Serialize(Database, options);
            File.WriteAllText(FilePath, updatedJson);
        }
    }
}
