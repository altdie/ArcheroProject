using System.Threading;
using Cysharp.Threading.Tasks;
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

        public async UniTask<PanelView> CreatePanelAsync(CancellationToken token)
        {
            _currentPanelView = await _assetProvider.LoadPanelPrefabAsync();
            token.ThrowIfCancellationRequested();
            _currentPanelView.transform.SetParent(_canvas.transform, false);
            _currentPanelView.gameObject.SetActive(true); 
            return _currentPanelView;
        }

        public void DestroyPanel()
        {
                Object.Destroy(_currentPanelView.gameObject); 
                _currentPanelView = null;
        }

    }
}