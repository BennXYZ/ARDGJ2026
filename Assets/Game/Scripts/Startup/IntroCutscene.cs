using UnityEngine;

namespace ArdJam2026.Startup
{
    public class IntroCutscene : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("After this time (in seconds), GoToMainMenu is called automatically, so we are not stuck forever.")]
        private float fallbackTimer = 20;
        private float? timer;

        private BootstrapGameState gameState;

        public void Initialize(BootstrapGameState gameState)
        {
            this.gameState = gameState;
            timer = fallbackTimer;
        }

        private void Update()
        {
            if (timer.HasValue)
            {
                timer -= Time.deltaTime;

                if (timer < 0)
                    gameState.GoToMainMenu();
            }
        }

        public void CutsceneOver()
        {
            gameState.GoToMainMenu();
        }
    }
}