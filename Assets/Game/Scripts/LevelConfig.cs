using SaintsField;
using UnityEngine;

namespace ArdJam2026
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField]
        private string title;

        [SerializeField]
        [Scene(true)]
        private string scene;

        public string Title => title;
        public string Scene => scene;
    }
}
