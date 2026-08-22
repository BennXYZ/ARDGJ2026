using UnityEngine;
using UnityEngine.Events;

namespace ArdJam2026.Gameplay
{
    public class Counter : RoomObject
    {
        private enum Comparison
        {
            Equals,
            Larger,
            LargerEquals
        }

        public UnityEvent TriggerChanged;
        public UnityEvent TriggerHit;
        public UnityEvent TriggerFromHit;

        [SerializeField]
        private Comparison comparison;

        [SerializeField]
        private int target = 2;
        public int Target => target;

        [SerializeField]
        private int value = 0;
        public int Value => value;

        protected override void Initialize()
        {
            if (IsTrigger(value))
                TriggerHit.Invoke();
        }

        public void Increase(int value) => SetValue(Value + value);

        public void Decrease(int value) => SetValue(Value - value);

        private void SetValue(int value)
        {
            if (this.value != value)
            {
                bool wasReached = IsTrigger(this.value);
                this.value = value;
                TriggerChanged.Invoke();
                if (!wasReached && IsTrigger(value))
                    TriggerHit.Invoke();
                else if (wasReached && !IsTrigger(value))
                    TriggerFromHit.Invoke();
            }
        }

        private bool IsTrigger(int value)
        {
            return comparison switch
            {
                Comparison.Equals => target == value,
                Comparison.Larger => target > value,
                Comparison.LargerEquals => target >= value,
                _ => false,
            };
        }
    }
}