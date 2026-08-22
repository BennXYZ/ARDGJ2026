using SaintsField.Playa;
using UnityEngine.Events;

namespace ArdJam2026.Gameplay
{
    public class FloorButton : RoomObject, IFloorButton
    {
        public UnityEvent Pressed;
        public UnityEvent Released;

        [ShowInInspector]
        public bool IsPressed { get; private set; }

        public void Press()
        {
            if (!IsPressed)
            {
                IsPressed = true;
                Pressed.Invoke();
            }
        }

        public void Release()
        {
            if (IsPressed)
            {
                IsPressed = false;
                Released.Invoke();
            }
        }
    }
}