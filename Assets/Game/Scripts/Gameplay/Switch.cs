using UnityEngine;
using UnityEngine.Events;

namespace ArdJam2026.Gameplay
{
    public class Switch : MonoBehaviour, IInteractible, ICollider
    {
        public UnityEvent On;
        public UnityEvent Off;

        [SerializeField]
        private bool state;

        private Room room;

        public Vector2Int Location { get; private set; }

        public bool IsStatic => true;

        public bool IsColliding => true;

        public bool State => state;

        public void Initialize(Room room)
        {
            if (this.room != room)
            {
                this.room = room;

                Location = room.GetLocation(transform);

                InvokeTriggers();
            }
        }

        public void Interact()
        {
            state = !state;

            InvokeTriggers();
        }

        private void InvokeTriggers()
        {
            if (state)
                On.Invoke();
            else
                Off.Invoke();
        }
    }
}