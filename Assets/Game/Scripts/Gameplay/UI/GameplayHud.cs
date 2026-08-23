using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace ArdJam2026.Gameplay.UI
{
    public class GameplayHud : MonoBehaviour
    {
        [SerializeField]
        private RectTransform container;

        [SerializeField]
        private float transitionDuration = 0.3f;

        [SerializeField]
        private UnityEvent onDeath;

        private GameplayGameState gameState;

        public void Initialize(GameplayGameState gameState)
        {
            this.gameState = gameState;
            Vector3 start = container.position;
            start.y = -container.sizeDelta.y;
            container.position = start;
        }

        public void Show()
        {
            container.DOMoveY(0, transitionDuration).SetEase(Ease.InQuad);
        }

        public void Hide()
        {
            container.DOMoveY(-container.sizeDelta.y, transitionDuration).SetEase(Ease.InQuad);
        }

        public void OpenPause()
        {
            gameState.Pause();
        }

        public void Interact()
        {
            gameState.DoInteractAction();
        }

        public void OnDeath()
        {
            onDeath.Invoke();
        }

        public void MoveDown() => gameState.DoMoveAction(Vector2Int.down);
        public void MoveLeft() => gameState.DoMoveAction(Vector2Int.left);
        public void MoveRight() => gameState.DoMoveAction(Vector2Int.right);
        public void MoveUp() => gameState.DoMoveAction(Vector2Int.up);
    }
}