using UnityEngine;

namespace ArdJam2026.Gameplay
{
    public interface IPushable
    {
        bool CanPush(Vector2Int direction);

        void Push(Vector2Int direction);
    }
}