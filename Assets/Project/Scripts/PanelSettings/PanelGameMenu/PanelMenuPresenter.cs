using Zenject;

namespace Project.Scripts.PanelSettings.PanelGameMenu
{
    public class PanelMenuPresenter : IInitializable
    {
        private readonly PanelMenuView _view;
        private readonly PanelMenuModel _model;

        public PanelMenuPresenter(PanelMenuView view, PanelMenuModel model)
        {
            _view = view;
            _model = model;
        }

        public void Initialize()
        {
            SubscribeOnClick();
        }

        private void SubscribeOnClick()
        {
            _view.RemoveAdsClicked += OnRemoveADSClicked;
            _view.StartGameClicked += OnStartGameClicked;
        }

        private void OnRemoveADSClicked()
        {
            _model.RemoveAds();
        }

        private void OnStartGameClicked()
        {
            _model.StartGame();
            Dispose();
        }

        private void Dispose()
        {
            _view.RemoveAdsClicked -= OnRemoveADSClicked;
            _view.StartGameClicked -= OnStartGameClicked;
        }
    }
}
