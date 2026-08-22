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

        [SerializeField] private float moveSpeed = 2;

        [ShowInInspector]
        public bool IsPossessed { get; private set; }

        [SerializeField, GetComponent] private Animatable animations;

        public int Speed => speed;

        public bool IsStatic => MoveCount < 1;

        public bool IsColliding => true;

        public int MoveCount { get; private set; }

        private readonly Queue<Vector3> movementTargets = new();
        private Vector3? currentMovementTarget;
        private bool movePawn;

        public bool IsMoving => movementTargets.Count > 0 || currentMovementTarget.HasValue;

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
                Vector3 moveDirection = (endPosition - currentPosition).normalized;
                movementTargets.Enqueue(currentPosition + moveDirection * 0.3f);
                movementTargets.Enqueue(currentPosition);
                PlayMoveAnimation(animationName);
                return;
            }

            if (Room.TryGetColliderAt(newPosition, out ICollider collider) && collider.IsColliding)
            {
                if (collider.IsStatic)
                {
                    MoveCount = 0;
                    Vector3 moveDirection = (endPosition - currentPosition).normalized;
                    movementTargets.Enqueue(currentPosition + moveDirection * 0.3f);
                    movementTargets.Enqueue(currentPosition);
                    PlayMoveAnimation(animationName);
                }

                if (collider is not IPushable pushable)
                    return;

                if (!pushable.CanPush(direction))
                {
                    MoveCount = 0;
                    Vector3 moveDirection = (endPosition - currentPosition).normalized;
                    movementTargets.Enqueue(currentPosition + moveDirection * 0.3f);
                    movementTargets.Enqueue(currentPosition);
                    PlayMoveAnimation(animationName);
                    return;
                }

                pushable.Push(direction);
            }

            MoveCount--;

            movementTargets.Enqueue(endPosition);
            Location = newPosition;
            if (MoveCount == 0)
            {
                PlayMoveAnimation(animationName);
            }

        }

        private void Update()
        {
            if (!movePawn)
                return;
            if (movementTargets.Count <= 0 && currentMovementTarget == null)
                return;

            currentMovementTarget ??= movementTargets.Dequeue();

            Vector3 direction = (currentMovementTarget.Value - transform.position).normalized;
            transform.Translate(direction * speed * moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, currentMovementTarget.Value) < 0.1f)
            {
                if (movementTargets.Count <= 0)
                {
                    transform.position = currentMovementTarget.Value;
                    movePawn = false;
                    MovementFinished();
                }

                currentMovementTarget = null;
            }
        }

        private void PlayMoveAnimation(string animationName)
        {
            animations.PlayAnimation(animationName, 0, PlayIdleAnimation, new Animatable.AnimationEvent(
                () =>
                {
                    movePawn = true;
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
            movementTargets.Clear();
            currentMovementTarget = null;
            movePawn = false;
            MoveCount = Speed;
        }
    }
}