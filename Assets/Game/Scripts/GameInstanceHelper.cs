using UnityEngine;

namespace ArdJam2026
{
    public class GameInstanceHelper : MonoBehaviour
    {
        private GameInstance gameInstance;

        public void Initialize(GameInstance gameInstance)
        {
            this.gameInstance = gameInstance;
        }

        private void OnDestroy()
        {
            gameInstance.StopGame();
        }
    }
}