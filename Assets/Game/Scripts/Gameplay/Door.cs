using UnityEngine;
using UnityEngine.Events;

namespace ArdJam2026.Gameplay
{
    public class Door : RoomObject, ICollider, ITurnHandler
    {
        public UnityEvent OnOpen;
        public UnityEvent OnClose;
        public UnityEvent OnInitOpen;
        public UnityEvent OnInitClosed;

        [SerializeField]
        private bool isOpen;

        private bool desiredIsOpen;

        public bool IsOpen => isOpen;

        public bool IsStatic => true;

        public bool IsColliding => !isOpen;

        protected override void Initialize()
        {
            desiredIsOpen = isOpen;
        }

        public override void PostInitialize()
        {
            if (!desiredIsOpen)
            {
                if (Room.TryGetColliderAt(Location, out _, this))
                    isOpen = true;
            }

            if (isOpen)
            {
                OnInitOpen.Invoke();
            }
            else
            {
                OnInitClosed.Invoke();
            }
        }

        public void Open()
        {
            SetOpen(true);
        }

        public void Close()
        {
            SetOpen(false);
        }

        public void Toggle()
        {
            SetOpen(!isOpen);
        }

        private void SetOpen(bool value)
        {
            desiredIsOpen = value;

            if (isOpen != value)
            {
                if (!value)
                {
                    if (Room.TryGetColliderAt(Location, out _, this))
                        value = true;
                }

                if (isOpen != value)
                {
                    isOpen = value;

                    if (isOpen)
                        OnOpen.Invoke();
                    else
                        OnClose.Invoke();
                }
            }
        }

        public void OnTurn()
        {
            if (!desiredIsOpen && isOpen)
            {
                SetOpen(false);
            }
        }
    }
}