using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ArdJam2026.Gameplay
{
    public class Room : MonoBehaviour
    {
        [SerializeField]
        private Tilemap level;
        private GameState gameState;

        public Tilemap Level => level;

        private readonly List<Pawn> pawns = new();
        private readonly List<IInteractible> interactibles = new();
        private readonly List<IFloorButton> floorButtons = new();
        private readonly List<ICollider> colliders = new();

        public void Initialize(GameState gameState)
        {
            this.gameState = gameState;
            MonoBehaviour[] behaviours = FindObjectsByType<MonoBehaviour>();

            pawns.Clear();
            interactibles.Clear();
            floorButtons.Clear();
            colliders.Clear();

            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour is Pawn pawn)
                {
                    pawn.Initialize(this);
                    pawns.Add(pawn);
                }
                if (behaviour is IInteractible interactible)
                {
                    interactible.Initialize(this);
                    interactibles.Add(interactible);
                }
                if (behaviour is IFloorButton floorButton)
                {
                    floorButton.Initialize(this);
                    floorButtons.Add(floorButton);
                }
                if (behaviour is ICollider collider)
                {
                    collider.Initialize(this);
                    colliders.Add(collider);
                }
            }
        }

        public void Possess(Vector3 position)
        {
            Vector3Int cell = level.WorldToCell(position);
            Debug.Log(cell);
            foreach (Pawn pawn in pawns)
            {
                if (!pawn.IsPossessed && pawn.Location == (Vector2Int)cell)
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
                pawn.PrepareMovement();
            }
            List<Pawn> movablePawns = new(pawns.Where(p => p.IsPossessed));

            int tries = movablePawns.Count * 10;
            while (movablePawns.Count > 0 && tries > 0)
            {
                for (int i = movablePawns.Count - 1; i >= 0; i--)
                {
                    if (movablePawns[i].IsStatic)
                    {
                        movablePawns.RemoveAtSwapBack(i);
                        continue;
                    }

                    movablePawns[i].Move(direction);
                }

                // Prevents endless loop
                tries--;
            }

            HashSet<Vector2Int> pawnLocations = new();
            foreach (Pawn pawn in pawns)
            {
                GameplayTile tile = GetTile(pawn.Location);
                if (tile && tile.Deadly)
                {
                    Debug.LogError("You're dead. Not big surprise", pawn);
                    gameState.GameOver();
                }
            }


            foreach (IFloorButton floorButton in floorButtons)
            {
                if (pawnLocations.Contains(floorButton.Location))
                    floorButton.Press();
                else if (floorButton.IsPressed)
                    floorButton.Release();
            }
        }

        public void PerformInteract()
        {
            foreach (Pawn pawn in pawns)
            {
                if (pawn.IsPossessed)
                {
                    pawn.Interact();

                    foreach (Vector2Int location in GetSurroundingCross(pawn.Location))
                    {
                        foreach (IInteractible interactible in interactibles)
                        {
                            if (interactible.Location == location)
                                interactible.Interact();
                        }
                    }
                }
            }
        }

        public GameplayTile GetTile(Vector2Int position)
        {
            if (level)
            {
                return level.GetTile<GameplayTile>((Vector3Int)position);
            }
            return null;
        }

        public bool TryGetColliderAt(Vector2Int position, out ICollider collider)
        {
            collider = default;
            foreach (ICollider checkedCollider in colliders)
            {
                if (checkedCollider.Location == position)
                {
                    collider = checkedCollider;
                    return true;
                }
            }
            return false;
        }

        public Vector2Int GetLocation(Transform transform)
        {
            return (Vector2Int)Level.WorldToCell(transform.position);
        }

        public IEnumerable<Vector2Int> GetSurroundingCross(Vector2Int location)
        {
            yield return location + Vector2Int.up;
            yield return location + Vector2Int.right;
            yield return location + Vector2Int.down;
            yield return location + Vector2Int.left;
        }

    }
}