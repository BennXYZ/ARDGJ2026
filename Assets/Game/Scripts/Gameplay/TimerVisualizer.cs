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
            [SerializeField]
            private string value;
            [SerializeField]
            private Sprite sprite;

            public char Value => value.Length > 0 ? value[0] : '\0';
            public Sprite Sprite => sprite;
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
                char value = digit.Value;
                Debug.Assert(value == '\0', "Digit has no value", this);
                if (value != '\0')
                    digitToChars[value] = digit.Sprite;
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