using UnityEngine;

namespace ArdJam2026.Gameplay
{
    public interface ICollider
    {
        bool IsStatic { get; }
        bool IsColliding { get; }

        Vector2Int Location { get; }

        void Initialize(Room room);
    }
}