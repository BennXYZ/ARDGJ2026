using SaintsField;
using UnityEngine;
using UnityEngine.Events;

namespace ArdJam2026.Gameplay.Sfx
{
    [RequireComponent(typeof(FloorButton))]
    public class FloorButtonListener : MonoBehaviour
    {
        public UnityEvent Pressed;
        public UnityEvent Released;

        [SerializeField]
        [GetComponent]
        private FloorButton floorButton;

        private void Awake()
        {
            floorButton.Pressed.AddListener(PressedCallback);
            floorButton.Released.AddListener(ReleasedCallback);
        }

        private void ReleasedCallback()
        {
            Released.Invoke();
        }

        private void PressedCallback()
        {
            Pressed.Invoke();
        }
    }
}