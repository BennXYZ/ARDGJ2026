using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ArdJam2026.Gameplay.UI
{
    public class GameplayHud : MonoBehaviour
    {
        [SerializeField]
        private RectTransform container;

        [SerializeField]
        private CanvasGroup titleContainer;

        [SerializeField]
        private TextMeshProUGUI titleText;

        [SerializeField]
        private float transitionDuration = 0.3f;

        [SerializeField]
        private UnityEvent onDeath;

        private GameplayGameState gameState;

        public void Initialize(GameplayGameState gameState)
        {
            this.gameState = gameState;
            titleText.SetText(gameState.CurrentLevelTitle);

            Vector3 start = container.position;
            start.y = -container.sizeDelta.y;
            container.position = start;

            titleContainer.blocksRaycasts = false;
            titleContainer.alpha = 0;
        }

        public void Show(bool withTitle)
        {
            container.DOMoveY(0, transitionDuration).SetEase(Ease.InQuad);
            if (withTitle)
            {
                titleContainer.blocksRaycasts = true;
                titleText.alpha = 0;
                Sequence sequence = DOTween.Sequence(this);
                sequence.Append(titleContainer.DOFade(1, 0.5f).SetEase(Ease.Linear));
                sequence.Append(titleText.DOFade(1, 0.5f).SetEase(Ease.Linear));
                sequence.Append(titleContainer.DOFade(0, 0.5f).SetEase(Ease.Linear).SetDelay(2));
                sequence.OnComplete(() => titleContainer.blocksRaycasts = false);
                sequence.Play();
            }
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