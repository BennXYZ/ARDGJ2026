using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArdJam2026.Gameplay
{
    public class Pawn : MonoBehaviour, ICollider
    {
        [SerializeField]
        private int speed = 1;

        private Room room;

        public bool IsPossessed { get; private set; }
        public Vector2Int Location { get; private set; }

        public int Speed => speed;

        public bool IsStatic => MoveCount == 0;

        public bool IsColliding => true;

        public int MoveCount { get; private set; }

        public void Interact()
        {
            // TODO: Trigger animation
        }

        public void Move(Vector2Int direction)
        {
            Vector2Int newPosition = Location + direction;
            GameplayTile tile = room.GetTile(newPosition);
            if (tile && tile.Collides)
            {
                MoveCount = 0;
                return;
            }

            if (room.TryGetColliderAt(newPosition, out ICollider collider) && collider.IsColliding)
            {
                if (collider.IsStatic)
                    MoveCount = 0;
                return;
            }

            MoveCount--;
            Location = newPosition;
            transform.position = room.Level.GetCellCenterWorld((Vector3Int)Location);
            RefreshLocation();

        }

        public void Possess()
        {
            IsPossessed = true;
            SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
            if (renderer)
                renderer.color = Color.chocolate;
        }

        public void Initialize(Room room)
        {
            if (this.room == room)
                return;

            this.room = room;
            RefreshLocation();
        }

        private void RefreshLocation()
        {
            Location = room.GetLocation(transform);
        }

        public void PrepareMovement()
        {
            MoveCount = Speed;
        }
    }
}