using SaintsField;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArdJam2026.Gameplay
{
    [RequireComponent(typeof(TimerVisualizer))]
    public class TimerVisualizer : MonoBehaviour
    {
        [Serializable]
        private class Digit
        {
            public string value;
            public Sprite sprite;
        }

        [GetComponent]
        [SerializeField]
        private Timer timer;

        [SerializeField]
        private List<Digit> digits;
        private readonly Dictionary<char, Sprite> digitToChars = new();

        [SerializeField]
        private List<SpriteRenderer> parts;

        [SerializeField]
        private string fallbackChar;

        private void Start()
        {
            Debug.Assert(digits.Count != 0, "No digits assigned", this);
            foreach (Digit digit in digits)
            {
                Debug.Assert(digit.value.Length == 1, "Digit Value must be 1", this);
                if (digit.value.Length > 0)
                    digitToChars[digit.value[0]] = digit.sprite;
            }

            timer.TimerChanged.AddListener(TimerChangedCallback);
            TimerChangedCallback(timer.Time);
        }

        private void TimerChangedCallback(int value)
        {
            char fallback = fallbackChar.Length > 0 ? fallbackChar[0] : '\0';
            string visualizedValue = value.ToString();
            for (int i = 0; i < parts.Count; i++)
            {
                char digit = visualizedValue.Length > i ? visualizedValue[^(i + 1)] : fallback;
                parts[i].sprite = digitToChars.GetValueOrDefault(digit, null);
            }
        }
    }
}