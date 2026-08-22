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
        private SceneReference menuScene;

        [SerializeField]
        private List<SceneReference> levels;

        [Header("Prefabs")]
        [SerializeField]
        private EventSystem eventSystem;

        [SerializeField]
        private GameplayHud gameplayHud;

        public GameplayHud GameplayHud => gameplayHud;

        public EventSystem EventSystem => eventSystem;

        public SceneReference MenuScene => menuScene;

        public List<SceneReference> Levels => levels;
    }
}