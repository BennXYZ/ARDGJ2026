using SaintsField;
using UnityEngine;

namespace ArdJam2026.Gameplay
{
    [RequireComponent(typeof(MoveAnimation))]
    public class Pushable : RoomObject, IPushable, ICollider
    {
        private bool omitAnimation;

        public bool IsStatic => false;

        public bool IsColliding => true;
        [SerializeField, GetComponent] private MoveAnimation moveAnimation;

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

                transform.position = Room.Level.GetCellCenterWorld((Vector3Int)Location);
                Location = newPosition;
                moveAnimation.Clear();
                moveAnimation.PushMovement(Room.Level.GetCellCenterWorld((Vector3Int)Location));
                moveAnimation.Play();

            }
            omitAnimation = false;
        }
    }
}