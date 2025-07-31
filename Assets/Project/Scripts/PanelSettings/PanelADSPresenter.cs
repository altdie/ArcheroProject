namespace Project.Scripts.PanelSettings
{
    public class PanelADSPresenter
    {
        private readonly PanelView _view;
        private readonly PanelADSModel _model;

        public PanelADSPresenter(PanelView view, PanelADSModel model)
        {
            _view = view;
            _model = model;
        }

        public void SubscribeOnClick()
        {
            _view.ReloadGameClicked += OnReloadClicked;
            _view.RewardedAdsClicked += OnFreeLifeClicked;
        }

        private void OnReloadClicked()
        {
            _model.ReloadGame();
            Dispose();
        }

        private void OnFreeLifeClicked()
        {
            _model.ShowRewardedAd();
            Dispose();
        }

        public void Dispose()
        {
            _view.ReloadGameClicked -= OnReloadClicked;
            _view.RewardedAdsClicked -= OnFreeLifeClicked;
        }
    }
}
