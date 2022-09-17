using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

using WPR.Common;

namespace Microsoft.Xna.Framework.GamerServices.TrueAchievements
{
    public class GameToKey
    {
        private Dictionary<string, string>? GameUrlMapping;
        private Dictionary<string, Dictionary<string, string>>? AchievementNameToKeyMapping;
        
        private const string ProductIdUrlJson = "ProductIdUrl.json";
        private const string AchievementMappingJson = "AchievementsNameToKey.json";

        private string JsonDataPath;

        public GameToKey()
        {
            JsonDataPath = Configuration.Current.DataPath("Database/TrueAchievements");
            Directory.CreateDirectory(JsonDataPath);

            try
            {
                GameUrlMapping = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(Path.Combine(JsonDataPath, ProductIdUrlJson)));
            } catch (Exception ex)
            {
                Log.Error(LogCategory.GamerServices, $"TrueAchievements ProductIdToUrl JSON list failed to load with exception:\n {ex}");
                GameUrlMapping = new Dictionary<string, string>();
            }

            try
            {
                AchievementNameToKeyMapping = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(
                    File.ReadAllText(Path.Combine(JsonDataPath, AchievementMappingJson)));
            }
            catch (Exception ex)
            {
                Log.Error(LogCategory.GamerServices, $"TrueAchievements AchivementsKeyToName JSON list failed to load with exception:\n {ex}");
                AchievementNameToKeyMapping = new Dictionary<string, Dictionary<string, string>>();
            }
        }

        public string? GetURLFromProductId(string productId)
        {
            return GameUrlMapping!.ContainsKey(productId) ? GameUrlMapping![productId] : null;
        }

        public string? GetAchievementKey(string productId, string name)
        {
            if (AchievementNameToKeyMapping!.ContainsKey(productId))
            {
                return AchievementNameToKeyMapping[productId].ContainsKey(name) ? AchievementNameToKeyMapping[productId][name] : null;
            }

            return null;
        }
    }
}
