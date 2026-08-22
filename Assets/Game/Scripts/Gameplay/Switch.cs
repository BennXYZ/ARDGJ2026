using UnityEngine;
using UnityEngine.Events;

namespace ArdJam2026.Gameplay
{
    public class Switch : RoomObject, IInteractible, ICollider
    {
        public UnityEvent On;
        public UnityEvent Off;
        public UnityEvent InteractOn;
        public UnityEvent InteractOff;
        public UnityEvent OnInteract;

        [SerializeField]
        private bool state;

        public bool IsStatic => true;

        public bool IsColliding => true;

        public bool State => state;

        protected override void Initialize()
        {
            InvokeTriggers();
        }

        public void Interact()
        {
            OnInteract.Invoke();

            state = !state;

            (state ? InteractOn : InteractOff).Invoke();

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