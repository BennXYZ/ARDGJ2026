using UnityEngine;
using UnityEngine.Events;

namespace ArdJam2026.Gameplay
{
    public class Timer : RoomObject, ITurnHandler
    {
        public UnityEvent TimeSet;
        public UnityEvent TimeUp;
        public UnityEvent<int> TimerChanged;

        [SerializeField]
        private int time;

        [SerializeField]
        private bool fireOnInitialize;

        public int Time => time;

        protected override void Initialize()
        {
            if (fireOnInitialize && time == 0)
                TimeUp.Invoke();
        }

        public void OnTurn()
        {
            if (time == 0)
                return;

            time--;
            TimerChanged.Invoke(time);
            if (time == 0)
                TimeUp.Invoke();
        }

        public void SetTimer(int time)
        {
            if (this.time != time)
            {
                this.time = time;
                TimerChanged.Invoke(this.time);
                TimeSet.Invoke();
            }
        }
    }
}