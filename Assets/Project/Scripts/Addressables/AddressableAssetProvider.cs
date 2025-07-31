using Cysharp.Threading.Tasks;
using Project.Scripts.PanelSettings;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Addressables
{
    public class AssetProvider : IAssetProvider
    {
        private const string PLAYER_PREFAB_ADDRESS = "Player";
        private const string PANEL_FREEGAME_PREFAB_ADRESS = "GameOverPanel";
        private const string BUTTON_ADS_ADDRESS = "ButtonADS";
        private const string PANEL_ENDGAME = "PanelGameEnd";

        public async UniTask<GameObject> CreatePlayerPrefab()
        {
            var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>(PLAYER_PREFAB_ADDRESS);
            await handle.Task;
            return handle.Result;
        }

        public async UniTask<PanelView> CreatePanelPrefabFreeLifeAsync()
        {
            var handle = UnityEngine.AddressableAssets.Addressables.InstantiateAsync(PANEL_FREEGAME_PREFAB_ADRESS);
            await handle.Task;

            var panelGO = handle.Result;
            var panelView = panelGO.GetComponent<PanelView>();

            return panelView;
        }

        public async UniTask<Button> CreateRewardAdsbAsync()
        {
            var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>(BUTTON_ADS_ADDRESS);
            await handle.Task;

            var prefab = handle.Result;
            var button = prefab.GetComponent<Button>();

            return button;
        }

        public async UniTask<PanelView> CreatePanelPrefabEndGameAsync()
        {
            var handle = UnityEngine.AddressableAssets.Addressables.InstantiateAsync(PANEL_ENDGAME);
            await handle.Task;

            var panelGO = handle.Result;
            var panelView = panelGO.GetComponent<PanelView>();

            return panelView;
        }
    }
}
