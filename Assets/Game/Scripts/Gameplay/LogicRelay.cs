using UnityEngine;
using UnityEngine.Events;

namespace ArdJam2026.Gameplay
{
    public class LogicRelay : MonoBehaviour
    {
        public UnityEvent Trigger;

        public void TriggerRelay()
        {
            Trigger.Invoke();
        }
    }
}