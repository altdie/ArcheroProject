using Project.Scripts.PlayerModels;

namespace Project.Scripts.UI
{
    public class PlayerStatsUIModel
    {
        private readonly PlayerModel _player;
        public int Level => _player.Level;
        public float Experience => _player.Experience;
        public float MaxExperience { get; set; }

        public PlayerStatsUIModel(PlayerModel player, float maxExperience)
        {
            _player = player;
            MaxExperience = maxExperience;
        }
    }
}