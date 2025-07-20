using System.Threading.Tasks;
using Project.Scripts.PanelSettings;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Addressables
{
    public interface IAssetProvider
    {
        Task<GameObject> LoadPlayerPrefabAsync();
        Task<PanelView> LoadPanelPrefabFreeLifeAsync();
        Task<PanelView> LoadPanelPrefabEndGameAsync();
        Task<Button> LoadRewardAdsbAsync();
    }
}