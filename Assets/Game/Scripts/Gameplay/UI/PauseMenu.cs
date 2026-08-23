using DG.Tweening;
using UnityEngine;

namespace ArdJam2026.Gameplay.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField]
        private PossessedIndicator possessedIndicator;

        [SerializeField]
        private CanvasGroup container;

        private GameplayGameState gameState;

        public void Initialize(GameplayGameState gameState)
        {
            this.gameState = gameState;

            possessedIndicator.Initialize(gameState);

            container.alpha = 0;
            container.blocksRaycasts = false;
        }

        public void Show()
        {
            possessedIndicator.Refresh();

            container.blocksRaycasts = true;
            container.DOFade(1, 0.1f);
            // TODO: Animate children
        }

        public void Hide()
        {
            container.blocksRaycasts = false;
            container.DOFade(0, 0.1f);
            // TODO: Animate children
        }

        public void Unpause()
        {
            gameState.Unpause();
        }

        public void Restart()
        {
            gameState.Restart();
        }

        public void Quit()
        {
            gameState.BackToMenu();
        }
    }
}