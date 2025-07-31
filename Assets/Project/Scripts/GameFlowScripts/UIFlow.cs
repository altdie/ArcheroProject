using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.ADS;
using Project.Scripts.PanelSettings;
using Project.Scripts.Players;
using Project.Scripts.UI;

namespace Project.Scripts.GameFlowScripts
{
    public class UIFlow
    {
        private PlayerStatsUIPresenter _playerStatsUIPresenter;
        private PlayerStatsUIModel _playerStatsUIModel;
        private PanelADSPresenter _panelPresenter;

        private readonly PlayerStatsUIView _playerStatsUIView;
        private readonly PanelFactory _panelFactory;

        public UIFlow(
            PlayerStatsUIView playerStatsUIView,
            PanelFactory panelFactory)
        {
            _playerStatsUIView = playerStatsUIView;
            _panelFactory = panelFactory;
        }

        public void Initialize(PlayerModel player, int maxExperience)
        {
            _playerStatsUIModel = new PlayerStatsUIModel(player, maxExperience);
            _playerStatsUIPresenter = new PlayerStatsUIPresenter(_playerStatsUIModel, _playerStatsUIView);
            _playerStatsUIPresenter.Initialize();
        }

        public void UpdatePlayerStatsUI()
        {
            _playerStatsUIPresenter?.UpdateView();
        }

        public void DestroyPanels()
        {
            _panelFactory.DestroyPanel();
        }

        public async UniTask ShowEndGamePanel(CancellationToken token, AdsService adsService, SceneLoader sceneLoader, Func<UniTask> reviveAction)
        {
            PanelView panelEndGame = await _panelFactory.CreatePanelEndGame(token);
            panelEndGame.AnimateIn();
            var panelModelEndGame = new PanelADSModel(adsService, sceneLoader, reviveAction);
            _panelPresenter = new PanelADSPresenter(panelEndGame, panelModelEndGame);
            _panelPresenter.SubscribeOnClick();
        }

        public async UniTask ShowFreeLifePanel(CancellationToken token, AdsService adsService, SceneLoader sceneLoader, Func<UniTask> reviveAction)
        {
            PanelView panelView = await _panelFactory.CreatePanelFreeLife(token);
            panelView.AnimateIn();
            var panelModel = new PanelADSModel(adsService, sceneLoader, reviveAction);
            _panelPresenter = new PanelADSPresenter(panelView, panelModel);
            _panelPresenter.SubscribeOnClick();
        }
    }
}