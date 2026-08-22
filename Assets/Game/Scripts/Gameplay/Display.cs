using UnityEngine;
using UnityEngine.Events;

namespace ArdJam2026.Gameplay
{
    public class Display : RoomObject
    {
        public UnityEvent On;
        public UnityEvent Off;

        [SerializeField]
        private bool isOn;

        protected override void Initialize()
        {
            InvokeTrigger();
        }

        public void Show()
        {
            SetIsOn(true);
        }

        public void Hide()
        {
            SetIsOn(false);
        }

        public void Toggle()
        {
            SetIsOn(!isOn);
        }

        private void SetIsOn(bool value)
        {
            if (isOn != value)
            {
                isOn = value;

                InvokeTrigger();
            }
        }

        private void InvokeTrigger()
        {
            if (isOn)
                On.Invoke();
            else
                Off.Invoke();
        }
    }
}