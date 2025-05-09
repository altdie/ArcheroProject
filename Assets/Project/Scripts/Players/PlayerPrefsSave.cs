using Project.Scripts.PlayerModels;
using UnityEngine;

namespace Project.Scripts.Players
{
    public class PlayerPrefsSave
    {
        private const string SaveKey = "PlayerSaveData";

        public void Save(PlayerModel player)
        {
            var data = new PlayerDataSave
            {
                Experience = player.Experience,
                Level = player.Level,
                IsAdsRemoved = player.IsAdsRemoved
            };

            Save(data);
        }

        public void Save(PlayerDataSave data)
        {
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }

        public PlayerDataSave Load()
        {
            if (!PlayerPrefs.HasKey(SaveKey))
            {
                return new PlayerDataSave();
            }

            var json = PlayerPrefs.GetString(SaveKey);
            return JsonUtility.FromJson<PlayerDataSave>(json);
        }

        public void Clear()
        {
            PlayerPrefs.DeleteKey(SaveKey);
            PlayerPrefs.Save();
        }
    }
}