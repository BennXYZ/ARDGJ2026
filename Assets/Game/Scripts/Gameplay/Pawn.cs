using SaintsField.Playa;
using UnityEngine;

namespace ArdJam2026.Gameplay
{
    public class Pawn : RoomObject, ICollider
    {
        [SerializeField]
        private int speed = 1;

        [ShowInInspector]
        public bool IsPossessed { get; private set; }

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
            GameplayTile tile = Room.GetTile(newPosition);
            if (tile && tile.Collides)
            {
                MoveCount = 0;
                return;
            }

            if (Room.TryGetColliderAt(newPosition, out ICollider collider) && collider.IsColliding)
            {
                if (collider.IsStatic)
                    MoveCount = 0;
                return;
            }

            MoveCount--;
            Location = newPosition;
            transform.position = Room.Level.GetCellCenterWorld((Vector3Int)Location);
            Location = Room.GetLocation(transform);

        }

        public void Possess()
        {
            IsPossessed = true;
            SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
            if (renderer)
                renderer.color = Color.chocolate;
        }

        public void PrepareMovement()
        {
            MoveCount = Speed;
        }
    }
}