using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI
{
    public class PlayerStatsUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Slider _experienceSlider;

        public void SetLevelText(string text)
        {
            _levelText.text = text;
        }

        public void SetExperienceProgress(float progress)
        {
            _experienceSlider.value = Mathf.Clamp01(progress);
        }
    }
}