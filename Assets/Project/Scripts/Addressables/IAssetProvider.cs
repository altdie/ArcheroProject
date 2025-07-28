using Cysharp.Threading.Tasks;
using Project.Scripts.PanelSettings;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Addressables
{
    public interface IAssetProvider
    {
        UniTask<GameObject> CreatePlayerPrefabAsync();
        UniTask<PanelView> CreatePanelPrefabFreeLifeAsync();
        UniTask<PanelView> CreatePanelPrefabEndGameAsync();
        UniTask<Button> CreateRewardAdsbAsync();
    }
}