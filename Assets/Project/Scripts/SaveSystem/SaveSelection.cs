using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Project.Scripts.PlayerModels;
using Project.Scripts.Players;
using Unity.Services.Core;

namespace Project.Scripts.SaveSystem
{
    public class SaveSelection
    {
        private readonly CloudSave _cloudSave;
        private readonly PlayerPrefsSave _localSave;

        public SaveSelection(PlayerPrefsSave localSave, CloudSave cloudSave)
        {
            _localSave = localSave;
            _cloudSave = cloudSave;
            InitializeUnityServices();
        }
        private async void InitializeUnityServices()
        {
            await UnityServices.InitializeAsync();
        }

        public async UniTask SaveAsync(PlayerModel data)
        {
            await _cloudSave.SaveToCloud(data);
            _localSave.Save(data);
        }

        public async Task<PlayerDataSave> LoadAsync()
        {
            var localData = _localSave.Load();
            var cloudData = await _cloudSave.LoadFromCloud();

            if (cloudData.LastSaved > localData.LastSaved)
                return cloudData;

            return localData;
        }

        public async UniTask ClearAsync(CancellationToken token)
        {
            await _cloudSave.ClearCloudSave();
            token.ThrowIfCancellationRequested();
            _localSave.Clear();
        }
    }
}
