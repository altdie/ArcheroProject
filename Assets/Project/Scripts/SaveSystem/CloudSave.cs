using System.Collections.Generic;
using Project.Scripts.PlayerModels;
using Project.Scripts.Players;
using Unity.Services.CloudSave;
using Cysharp.Threading.Tasks;

namespace Project.Scripts.SaveSystem
{
    public class CloudSave
    {
        public async UniTask SaveToCloud(PlayerModel data)
        {
            var saveDict = new Dictionary<string, object>
            {
                { "Experience", data.Experience },
                { "Level", data.Level },
                { "IsAdsRemoved", data.IsAdsRemoved },
                { "LastSaved", data.LastSave }
            };

            await CloudSaveService.Instance.Data.Player.SaveAsync(saveDict).AsUniTask();
        }

        public async UniTask<PlayerDataSave> LoadFromCloud()
        {
            var keys = new HashSet<string> { "Experience", "Level", "IsAdsRemoved", "LastSaved" };
            var result = await CloudSaveService.Instance.Data.LoadAsync(keys).AsUniTask();

            return new PlayerDataSave
            {
                Experience = int.Parse(result["Experience"]),
                Level = int.Parse(result["Level"]),
                IsAdsRemoved = bool.Parse(result["IsAdsRemoved"]),
                LastSaved = int.Parse(result["LastSaved"])
            };
        }

        public async UniTask ClearCloudSave()
        {
            var saveDict = new Dictionary<string, object>
            {
                { "Experience", 0 },
                { "Level", 0 },
                { "IsAdsRemoved", false },
                { "LastSaved", 0L }
            };

            await CloudSaveService.Instance.Data.Player.SaveAsync(saveDict).AsUniTask();
        }
    }
}
