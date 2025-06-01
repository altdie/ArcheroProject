using System.Threading.Tasks;
using Project.Scripts.Addressables;
using UnityEngine;

namespace Project.Scripts.PanelSettings
{
    public class PanelFactory
    {
        private PanelView _currentPanelView;
        private readonly IAssetProvider _assetProvider;
        private readonly Canvas _canvas;

        public PanelFactory(Canvas canvas, IAssetProvider assetProvider)
        {
            _canvas = canvas;
            _assetProvider = assetProvider;
        }

        public async Task CreatePanelAsync()
        {
            _currentPanelView = await _assetProvider.LoadPanelPrefabAsync();
            _currentPanelView.transform.SetParent(_canvas.transform, false);
            _currentPanelView.gameObject.SetActive(true);
        }
    }
}