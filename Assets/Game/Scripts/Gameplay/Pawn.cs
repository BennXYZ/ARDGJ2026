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

        public int Speed => speed;

        public bool IsStatic => MoveCount < 1;

        public bool IsColliding => true;

        public int MoveCount { get; private set; }

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

        public void Move(Vector2Int direction)
        {
            Vector2Int newPosition = Location + direction;
            Vector3 startPosition = Room.Level.GetCellCenterWorld((Vector3Int)Location);
            Vector3 endPosition = Room.Level.GetCellCenterWorld((Vector3Int)newPosition);
            string animationName = "Possessed_Idle";
            if (newPosition.x > Location.x)
                animationName = "Move_Right";
            if (newPosition.x < Location.x)
                animationName = "Move_Left";
            if (newPosition.y > Location.y)
                animationName = "Move_Up";
            if (newPosition.y < Location.y)
                animationName = "Move_Down";

            GameplayTile tile = Room.GetTile(newPosition);
            if (tile && tile.Collides)
            {
                MoveCount = 0;
                PlayBlockedMoveAnimation(animationName, startPosition, endPosition);
                return;
            }

            if (Room.TryGetColliderAt(newPosition, out ICollider collider) && collider.IsColliding)
            {
                if (collider.IsStatic)
                {
                    MoveCount = 0;
                    PlayBlockedMoveAnimation(animationName, startPosition, endPosition);
                    return;
                }

                if (collider is not IPushable pushable)
                    return;

                if (!pushable.CanPush(direction))
                {
                    MoveCount = 0;
                    PlayBlockedMoveAnimation(animationName, startPosition, endPosition);
                    return;
                }

                pushable.Push(direction);
            }

            MoveCount--;

            PlayMoveAnimation(animationName, startPosition, endPosition);

            Location = newPosition;

        }

        private void PlayBlockedMoveAnimation(string animationName, Vector3 from, Vector3 to)
        {
            transform.DOKill();
            transform.position = from;
            to = from + (to - from).normalized * (to - from).magnitude * 0.4f;
            animations.PlayAnimation(animationName, 0, PlayIdleAnimation, new Animatable.AnimationEvent(
                () =>
                {
                    transform.DOMove(to, 0.15f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
                }, 3));
        }

        private void PlayMoveAnimation(string animationName, Vector3 from, Vector3 to)
        {
            transform.DOKill();
            transform.position = from;
            animations.PlayAnimation(animationName, 0, PlayIdleAnimation, new Animatable.AnimationEvent(
                () =>
                {
                    transform.DOMove(to, 0.3f).SetEase(Ease.Linear).OnComplete(MovementFinished);
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
            MoveCount = Speed;
        }
    }
}