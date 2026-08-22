using UnityEngine;

namespace ArdJam2026.Gameplay
{
    public interface IFloorButton
    {
        bool IsPressed { get; }
        Vector2Int Location { get; }

        void Initialize(Room room);

        void Press();
        void Release();
    }
}