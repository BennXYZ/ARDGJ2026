using System.Collections.Generic;
using DG.Tweening;
using SaintsField;
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

        [SerializeField, GetComponent] private Animatable animations;
        [SerializeField, GetComponent] private MoveAnimation moveAnimation;

        public int Speed => speed;

        public bool IsStatic => MoveCount < 1;

        public bool IsColliding => true;

        public int MoveCount { get; private set; }

        public bool IsMoving => moveAnimation == null || (moveAnimation.IsMoving);

        public void Interact()
        {
            if (animations)
                animations.PlayAnimation("Interact", 0, () =>
                {
                    PlayIdleAnimation();
                    InteractionFinished();
                });
        }

        private void InteractionFinished()
        {
            //TODO(bz): movement finished, update interactables
        }

        protected override void Initialize()
        {
            if (moveAnimation)
                moveAnimation.moveSpeed *= speed;
        }

        public void Move(Vector2Int direction)
        {
            Vector2Int newPosition = Location + direction;
            Vector3 currentPosition = Room.Level.GetCellCenterWorld((Vector3Int)Location);
            Vector3 endPosition = Room.Level.GetCellCenterWorld((Vector3Int)newPosition);

            string animationName = "Possessed_Idle";
            if (direction.x > 0)
                animationName = "Move_Right";
            if (direction.x < 0)
                animationName = "Move_Left";
            if (direction.y > 0)
                animationName = "Move_Up";
            if (direction.y < 0)
                animationName = "Move_Down";

            GameplayTile tile = Room.GetTile(newPosition);
            if (tile && tile.Collides)
            {
                MoveCount = 0;
                PushBlockedMovement(currentPosition, endPosition);
                PlayMoveAnimation(animationName);
                return;
            }

            if (Room.TryGetColliderAt(newPosition, out ICollider collider) && collider.IsColliding)
            {
                if (collider.IsStatic)
                {
                    MoveCount = 0;
                    PushBlockedMovement(currentPosition, endPosition);
                    PlayMoveAnimation(animationName);
                }

                if (collider is not IPushable pushable)
                    return;

                if (!pushable.CanPush(direction))
                {
                    MoveCount = 0;
                    PushBlockedMovement(currentPosition, endPosition);
                    PlayMoveAnimation(animationName);
                    return;
                }

                pushable.Push(direction);
            }

            MoveCount--;

            moveAnimation?.PushMovement(endPosition);
            Location = newPosition;
            if (MoveCount == 0)
            {
                PlayMoveAnimation(animationName);
            }
        }

        private void PushBlockedMovement(Vector3 currentPosition, Vector3 endPosition)
        {
            Vector3 moveDirection = (endPosition - currentPosition).normalized;
            moveAnimation?.PushMovement(currentPosition + moveDirection * 0.3f);
            moveAnimation?.PushMovement(currentPosition);
        }

        private void PlayMoveAnimation(string animationName)
        {
            animations.PlayAnimation(animationName, 0, PlayIdleAnimation, new Animatable.AnimationEvent(
                () =>
                {
                    moveAnimation?.Play();
                }, 3));
        }

        private void MovementFinished()
        {
            //TODO(bz): movement finished, update interactables
        }

        private void PlayIdleAnimation()
        {
            animations.PlayAnimation(IsPossessed ? "Possessed_Idle" : "Normal_Idle");
        }

        public void Possess()
        {
            IsPossessed = true;
            if (animations)
                animations.PlayAnimation("Possess", 0, PlayIdleAnimation);
        }

        public void PrepareMovement()
        {
            transform.position = Room.Level.GetCellCenterWorld((Vector3Int)Location);
            moveAnimation?.Clear();
            MoveCount = Speed;
        }
    }
}