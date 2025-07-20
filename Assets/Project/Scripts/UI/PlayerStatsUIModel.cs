using Project.Scripts.PlayerModels;

namespace Project.Scripts.UI
{
    public class PlayerStatsUIModel
    {
        public PlayerModel PlayerModel;
        public int Level => PlayerModel.Level;
        public float Experience => PlayerModel.Experience;
        public float MaxExperience { get; set; }

        public PlayerStatsUIModel(PlayerModel player, float maxExperience)
        {
            PlayerModel = player;
            MaxExperience = maxExperience;
        }
    }
}