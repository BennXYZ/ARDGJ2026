using System.Collections.Generic;
using UnityEngine;

namespace ArdJam2026.Gameplay.UI
{
    public class PossessedIndicator : MonoBehaviour
    {
        private readonly List<PossessedIndicatorItem> items = new();

        [SerializeField]
        private PossessedIndicatorItem prefab;

        [SerializeField]
        private RectTransform container;

        public void Initialize(GameplayGameState gameState)
        {
            foreach (Pawn pawn in gameState.CurrentRoom.Pawns)
            {
                PossessedIndicatorItem item = Instantiate(prefab, container);
                item.Initialize(pawn);
                items.Add(item);
            }
        }

        public void Refresh()
        {
            foreach (PossessedIndicatorItem item in items)
            {
                item.Refresh();
            }
        }
    }
}