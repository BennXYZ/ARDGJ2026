using UnityEngine;
using UnityEngine.Events;

namespace ArdJam2026.Gameplay
{
    public class Switch : RoomObject, IInteractible, ICollider
    {
        public UnityEvent On;
        public UnityEvent Off;

        [SerializeField]
        private bool state;

        [SerializeField] private Animator animator;

        public bool IsStatic => true;

        public bool IsColliding => true;

        public bool State => state;

        protected override void Initialize()
        {
            InvokeTriggers();
        }

        public void Interact()
        {
            state = !state;

            animator.Play(state ? "Lever_Activate" : "Lever_Deactivate");

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