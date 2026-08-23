using ArdJam2026.Gameplay.UI;
using SaintsField;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ArdJam2026
{
    [CreateAssetMenu(menuName = "Game/Game Config", fileName = "GameConfig")]
    public class GameConfiguration : ScriptableObject
    {
        [Header("Scenes")]
        [SerializeField]
        [Scene(true)]
        private string startupScene;

        [SerializeField]
        [Scene(true)]
        private string menuScene;

        [SerializeField]
        private List<LevelConfig> levels;

        [Header("Prefabs")]
        [SerializeField]
        private EventSystem eventSystem;

        [SerializeField]
        private GameplayHud gameplayHud;

        [SerializeField]
        private PauseMenu pauseMenu;

        public PauseMenu PauseMenu => pauseMenu;

        public GameplayHud GameplayHud => gameplayHud;

        public EventSystem EventSystem => eventSystem;

        public string StartupScene => startupScene;
        public string MenuScene => menuScene;

        public List<LevelConfig> Levels => levels;
    }
}