using ArdJam2026.Gameplay;
using DG.Tweening;
using UnityEngine;

public class GameOverHud : MonoBehaviour
{
    private GameplayGameState gameState;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Transform buttonParent;

    public void Initialize(GameplayGameState gameState)
    {
        this.gameState = gameState;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
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
