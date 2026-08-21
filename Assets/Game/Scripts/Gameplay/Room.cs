using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ArdJam2026.Gameplay
{
    public class Room : MonoBehaviour
    {
        [SerializeField]
        private Tilemap gameplay;

        private readonly List<Pawn> pawns = new();

        public void Initialize(GameState gameState)
        {

        }

        public void Possess(Vector3 position)
        {
            Vector3Int cell = gameplay.WorldToCell(position);
            foreach (Pawn pawn in pawns)
            {
                if (pawn.Location == ((Vector2Int)cell))
                {
                    pawn.Possess();
                    break;
                }
            }
        }

        public void PerformMove(Vector2Int direction)
        {
            foreach (Pawn pawn in pawns)
            {
                if (pawn.IsPossessed)
                    pawn.Move(direction);
            }
        }

        public void PerformInteract()
        {
            foreach (Pawn pawn in pawns)
            {
                if (pawn.IsPossessed)
                    pawn.Interact();
            }
        }
    }
}