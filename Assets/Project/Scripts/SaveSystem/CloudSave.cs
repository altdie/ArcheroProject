using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Scripts.PlayerModels;
using Project.Scripts.Players;
using Unity.Services.CloudSave;

namespace Project.Scripts
{
    public class CloudSave
    {
        public async Task SaveToCloud(PlayerModel data)
        {
            var saveDict = new Dictionary<string, object>
            {
                { "Experience", data.Experience },
                { "Level", data.Level },
                { "IsAdsRemoved", data.IsAdsRemoved },
                { "LastSaved", data.LastSave }
            };

            await CloudSaveService.Instance.Data.Player.SaveAsync(saveDict);
        }

        public async Task<PlayerDataSave> LoadFromCloud()
        {
            var keys = new HashSet<string> { "Experience", "Level", "IsAdsRemoved", "LastSaved" };
            var result = await CloudSaveService.Instance.Data.LoadAsync(keys);

            return new PlayerDataSave
            {
                Experience = int.Parse(result["Experience"]),
                Level = int.Parse(result["Level"]),
                IsAdsRemoved = bool.Parse(result["IsAdsRemoved"]),
                LastSaved = int.Parse(result["LastSaved"])
            };
        }

        public async Task ClearCloudSave()
        {
            var saveDict = new Dictionary<string, object>
            {
                { "Experience", 0 },
                { "Level", 0 },
                { "IsAdsRemoved", false },
                { "LastSaved", 0L }
            };

            await CloudSaveService.Instance.Data.Player.SaveAsync(saveDict);
        }
    }
}
