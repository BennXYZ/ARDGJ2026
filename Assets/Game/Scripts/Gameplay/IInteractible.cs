using UnityEngine;

namespace ArdJam2026.Gameplay
{
    public interface IInteractible
    {
        Vector2Int Location { get; }

        void Initialize(Room room);

        void Interact();
    }
}