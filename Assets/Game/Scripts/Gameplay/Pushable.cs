using UnityEngine;

namespace ArdJam2026.Gameplay
{
    public class Pushable : RoomObject, IPushable, ICollider
    {
        private bool omitAnimation;

        public bool IsStatic => false;

        public bool IsColliding => true;

        public bool CanPush(Vector2Int direction)
        {
            Vector2Int newPosition = Location + direction;

            GameplayTile tile = Room.GetTile(newPosition);
            if (tile && tile.Collides)
            {
                // TODO: Block Animation
                return false;
            }

            if (Room.TryGetColliderAt(newPosition, out ICollider collider) && collider.IsColliding)
            {
                // TODO: Block Animation
                return false;
            }

            return true;
        }

        public void Push(Vector2Int direction)
        {
            omitAnimation = true;
            if (CanPush(direction))
            {
                Vector2Int newPosition = Location + direction;

                omitAnimation = false;
                // TODO: Move Animation

                Location = newPosition;

                transform.position = Room.Level.GetCellCenterWorld((Vector3Int)Location);
            }
            omitAnimation = false;
        }
    }
}