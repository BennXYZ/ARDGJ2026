using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimation : MonoBehaviour
{
    public float moveSpeed = 3;
    private readonly Queue<Vector3> movementTargets = new();
    private Vector3? currentMovementTarget;
    public MoveStates MoveState { get; private set; }

    public bool IsMoving => MoveState != MoveStates.Idle;

    private void Update()
    {
        if (MoveState != MoveStates.Moving)
            return;
        if (movementTargets == null)
            return;
        if (movementTargets.Count <= 0 && !currentMovementTarget.HasValue)
            return;

        currentMovementTarget ??= movementTargets.Dequeue();

        Vector3 direction = (currentMovementTarget.Value - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, currentMovementTarget.Value) < 0.1f)
        {
            if (movementTargets.Count <= 0)
            {
                transform.position = currentMovementTarget.Value;
                MoveState = MoveStates.Idle;
                MovementFinished();
            }

            currentMovementTarget = null;
        }
    }

    public void Play()
    {
        if (MoveState == MoveStates.Preparation)
            MoveState = MoveStates.Moving;
    }

    public void PushMovement(Vector3 target)
    {
        movementTargets.Enqueue(target);
        if (MoveState == MoveStates.Idle)
            MoveState = MoveStates.Preparation;
    }

    public void Clear()
    {
        movementTargets.Clear();
        currentMovementTarget = null;
        MoveState = MoveStates.Idle;
    }

    public void MovementFinished()
    {

    }

    public enum MoveStates
    {
        Idle = 0,
        Preparation,
        Moving,
    }
}
