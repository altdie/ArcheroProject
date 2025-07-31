using System.Threading;
using Cysharp.Threading.Tasks;
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
        }

        public async UniTask Initialize()
        {
            await UnityServices.InitializeAsync();
        }

        public async UniTask Save(PlayerModel data)
        {
            await _cloudSave.SaveToCloud(data);
            _localSave.Save(data);
        }

        public async UniTask<PlayerDataSave> Load(CancellationToken token)
        {
            var localData = _localSave.Load();
            var cloudData = await _cloudSave.LoadFromCloud();
            
            token.ThrowIfCancellationRequested();

            if (cloudData.LastSaved > localData.LastSaved)
            {
                return cloudData;
            }

            return localData;
        }

        public async UniTask Clear(CancellationToken token)
        {
            await _cloudSave.ClearCloudSave();
            token.ThrowIfCancellationRequested();
            _localSave.Clear();
        }
    }
}