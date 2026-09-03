using ArdJam2026.Gameplay;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class GameOverHud : MonoBehaviour
{
    private GameplayGameState gameState;

    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Transform buttonParent;

    public UnityEvent<bool> HasNextLevel;

    public void Initialize(GameplayGameState gameState)
    {
        this.gameState = gameState;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        HasNextLevel.Invoke(gameState.HasNextLevel);
    }

    public void Show()
    {
        canvasGroup.DOFade(1, 1f);
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        buttonParent.transform.position += Vector3.down * 200;
        buttonParent.transform.DOMove(Vector3.up * 200, 0.7f).SetRelative(true).SetDelay(1f);
    }

    public void StartNextLevel()
    {
        gameState.NextLevel();
    }

    public void RestartLevel()
    {
        gameState.Restart();
    }

    public void BackToMenu()
    {
        gameState.BackToMenu();
    }
}
