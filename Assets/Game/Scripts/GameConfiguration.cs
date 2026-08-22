using SaintsField;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ArdJam2026
{
    [CreateAssetMenu(menuName = "Game/Game Config", fileName = "GameConfig")]
    public class GameConfiguration : ScriptableObject
    {
        [SerializeField]
        private SceneReference menuScene;

        [SerializeField]
        private List<SceneReference> levels;

        [SerializeField]
        private EventSystem eventSystem;

        public EventSystem EventSystem => eventSystem;

        public SceneReference MenuScene => menuScene;

        public List<SceneReference> Levels => levels;
    }
}