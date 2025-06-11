namespace Project.Scripts.UI
{
    public class PlayerStatsUIPresenter
    {
        private readonly PlayerStatsUIModel _uiModel;
        private readonly PlayerStatsUIView _uiView;

        public PlayerStatsUIPresenter(PlayerStatsUIModel uiModel, PlayerStatsUIView uiView)
        {
            _uiModel = uiModel;
            _uiView = uiView;

            UpdateView();
        }

        public void UpdateView()
        {
            _uiView.SetLevelText("Level: " + _uiModel.Level);

            float progress = 0f;
            if (_uiModel.MaxExperience > 0)
                progress = _uiModel.Experience / _uiModel.MaxExperience;

            _uiView.SetExperienceProgress(progress);
        }
    }
}
