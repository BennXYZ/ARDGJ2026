using SaintsField.Playa;
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
        private bool turnStarted;
        private bool pawnDied;

        public Tilemap Level => level;

        [ShowInInspector]
        private readonly List<Pawn> pawns = new();
        private readonly List<IInteractible> interactibles = new();
        private readonly List<IFloorButton> floorButtons = new();
        private readonly List<ICollider> colliders = new();
        private readonly List<ITurnHandler> turnHandlers = new();

        public void Initialize(GameState gameState)
        {
            this.gameState = gameState;
            RoomObject[] objects = FindObjectsByType<RoomObject>();

            pawns.Clear();
            interactibles.Clear();
            floorButtons.Clear();
            colliders.Clear();
            turnHandlers.Clear();

            foreach (RoomObject roomObject in objects)
            {
                if (roomObject is Pawn pawn)
                    pawns.Add(pawn);
                if (roomObject is IInteractible interactible)
                    interactibles.Add(interactible);
                if (roomObject is IFloorButton floorButton)
                    floorButtons.Add(floorButton);
                if (roomObject is ICollider collider)
                    colliders.Add(collider);
                if (roomObject is ITurnHandler turnHandler)
                    turnHandlers.Add(turnHandler);
                roomObject.Initialize(this);
            }

            foreach (RoomObject roomObject in objects)
            {
                roomObject.PostInitialize();
            }

            // Maybe the initial editing placed something on a button
            PostMoveChecks();
        }

        public void Possess(Vector3 position)
        {
            if (turnStarted)
                return;

            turnStarted = true;

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
            if (turnStarted)
                return;

            turnStarted = true;
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

                    CheckForDeath(movablePawns[i]);
                }

                // Prevents endless loop
                tries--;
            }
        }

        private void CheckForDeath(Pawn pawn)
        {
            GameplayTile tile = GetTile(pawn.Location);
            if (tile && tile.Deadly)
            {
                Debug.LogError("You're dead. Not big surprise", pawn);
                pawnDied = true;
            }

        }

        private void Update()
        {
            if (turnStarted)
            {
                turnStarted = pawns.Any(p => p.IsMoving);
                if (!turnStarted)
                {
                    PostMoveChecks();
                    OnTurnComplete();
                }
            }
        }

        public void PerformInteract()
        {
            if (turnStarted)
                return;

            turnStarted = true;

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

        private void OnTurnComplete()
        {
            if (pawnDied)
                gameState.GameOver();

            foreach (ITurnHandler turnHandler in turnHandlers)
            {
                turnHandler.OnTurn();
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

        public bool TryGetColliderAt(Vector2Int position, out ICollider collider, ICollider self = default)
        {
            collider = default;
            foreach (ICollider checkedCollider in colliders)
            {
                if (checkedCollider == self)
                    continue;

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

        private void PostMoveChecks()
        {
            HashSet<Vector2Int> colliderLocations = new(colliders.Select(c => c.Location));
            foreach (IFloorButton floorButton in floorButtons)
            {
                if (colliderLocations.Contains(floorButton.Location))
                    floorButton.Press();
                else if (floorButton.IsPressed)
                    floorButton.Release();
            }
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