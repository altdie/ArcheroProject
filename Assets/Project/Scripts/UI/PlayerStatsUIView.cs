using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI
{
    public class PlayerStatsUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Slider experienceSlider;

        public void SetLevelText(string text)
        {
            levelText.text = text;
        }

        public void SetExperienceProgress(float progress)
        {
            experienceSlider.value = Mathf.Clamp01(progress);
        }
    }
}