using SaintsField.Playa;
using UnityEngine;

namespace ArdJam2026.Gameplay
{
    public abstract class RoomObject : MonoBehaviour
    {
        [ShowInInspector]
        public Vector2Int Location { get; protected set; }
        [ShowInInspector]
        public Room Room { get; private set; }

        public void Initialize(Room room)
        {
            if (Room != room)
            {
                Room = room;

                if (Room)
                {
                    Location = room.GetLocation(transform);
                    Initialize();
                }
            }
        }

        protected virtual void Initialize() { }
    }
}