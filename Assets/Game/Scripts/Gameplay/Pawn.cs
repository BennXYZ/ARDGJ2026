using System;
using System.Collections.Generic;
using DG.Tweening;
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

        private Animatable animations;

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

            Vector3 startPosition = Room.Level.GetCellCenterWorld((Vector3Int)Location);
            Vector3 endPosition = Room.Level.GetCellCenterWorld((Vector3Int)newPosition);
            if (newPosition.x > Location.x)
                PlayMoveAnimation("Move_Right", startPosition, endPosition);
            if (newPosition.x < Location.x)
                PlayMoveAnimation("Move_Left", startPosition, endPosition);
            if (newPosition.y > Location.y)
                PlayMoveAnimation("Move_Up", startPosition, endPosition);
            if (newPosition.y < Location.y)
                PlayMoveAnimation("Move_Down", startPosition, endPosition);

            Location = newPosition;

        }

        private void PlayMoveAnimation(string animationName, Vector3 from, Vector3 to)
        {
            transform.DOKill();
            transform.position = from;
            animations.PlayAnimation(animationName, 0, PlayIdleAnimation, new Animatable.AnimationEvent(
                () =>
                {
                    transform.DOMove(to, 0.3f).SetEase(Ease.Linear);
                },3));
        }

        private void PlayIdleAnimation()
        {
            animations.PlayAnimation(IsPossessed ? "Possessed_Idle" : "Normal_Idle");
        }

        public void Possess()
        {
            IsPossessed = true;
            if(animations)
                animations.PlayAnimation("Possess", 0, PlayIdleAnimation);
        }

        protected override void Initialize()
        {
            animations = GetComponent<Animatable>();
            base.Initialize();
        }

        public void PrepareMovement()
        {
            MoveCount = Speed;
        }
    }
}