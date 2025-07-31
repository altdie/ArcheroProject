using Cysharp.Threading.Tasks;
using Project.Scripts.PanelSettings;
using UnityEngine;

namespace Project.Scripts.Addressables
{
    public interface IAssetProvider
    {
        UniTask<GameObject> CreatePlayerPrefab();
        UniTask<PanelView> CreatePanelPrefabFreeLifeAsync();
        UniTask<PanelView> CreatePanelPrefabEndGameAsync();
    }
}