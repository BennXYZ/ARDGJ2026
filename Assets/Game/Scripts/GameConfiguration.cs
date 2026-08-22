using SaintsField;
using System.Collections.Generic;
using UnityEngine;

namespace ArdJam2026
{
    [CreateAssetMenu(menuName = "Game/Game Config", fileName = "GameConfig")]
    public class GameConfiguration : ScriptableObject
    {
        [SerializeField]
        private SceneReference menuScene;

        [SerializeField]
        private List<SceneReference> levels;

        public SceneReference MenuScene => menuScene;

        public List<SceneReference> Levels => levels;
    }
}