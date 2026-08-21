using System;
using UnityEngine;

namespace ArdJam2026.Gameplay
{
    internal class Pawn
    {
        public bool IsPossessed { get; internal set; }
        public Vector2Int Location { get; internal set; }

        public void Interact()
        {
            throw new NotImplementedException();
        }

        public void Move(Vector2Int direction)
        {
            throw new NotImplementedException();
        }

        public void Possess()
        {
            throw new NotImplementedException();
        }
    }
}