using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArdJam2026.MainMenu
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        [SerializeField]
        private TextMeshProUGUI title;

        private LevelSelect levelSelect;
        private LevelConfig level;

        public void Initialize(LevelSelect levelSelect)
        {
            this.levelSelect = levelSelect;
            SetLevel(null);
        }

        public void SetLevel(LevelConfig level)
        {
            this.level = level;
            button.interactable = level;
            title.SetText(level ? level.Title : string.Empty);
        }

        public void StartLevel()
        {
            levelSelect.StartLevel(level);
        }
    }
}