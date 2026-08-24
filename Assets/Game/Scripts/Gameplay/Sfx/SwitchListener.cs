using SaintsField;
using UnityEngine;
using UnityEngine.Events;

namespace ArdJam2026.Gameplay.Sfx
{
    [RequireComponent(typeof(Switch))]
    public class SwitchListener : MonoBehaviour
    {
        public UnityEvent On;
        public UnityEvent Off;
        public UnityEvent InteractOn;
        public UnityEvent InteractOff;
        public UnityEvent OnInteract;

        [SerializeField]
        [GetComponent]
        private Switch switchReference;

        private void Awake()
        {
            switchReference.On.AddListener(OnCallback);
            switchReference.Off.AddListener(OffCallback);
            switchReference.InteractOn.AddListener(InteractOnCallback);
            switchReference.InteractOff.AddListener(InteractOffCallback);
            switchReference.OnInteract.AddListener(OnInteractCallback);
        }

        private void OnInteractCallback()
        {
            OnInteract.Invoke();
        }

        private void InteractOffCallback()
        {
            InteractOff.Invoke();
        }

        private void InteractOnCallback()
        {
            InteractOn.Invoke();
        }

        private void OffCallback()
        {
            Off.Invoke();
        }

        private void OnCallback()
        {
            On.Invoke();
        }
    }
}