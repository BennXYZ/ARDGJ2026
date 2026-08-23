using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ArdJam2026.Gameplay
{
    public class Tutorial : RoomObject
    {
        [Serializable]
        public class TutorialData
        {
            [TextArea]
            public string Text;
        }

        private int index = -1;

        [SerializeField]
        private List<TutorialData> data;

        [SerializeField]
        private CanvasGroup tutorialBox;

        [SerializeField]
        private TextMeshProUGUI text;

        private bool acceptClick = false;

        protected override void Initialize()
        {
            tutorialBox.alpha = 0;
            text.alpha = 0;
            if (data.Count > 0)
            {
                Room.GameState.SetTutorial(this);
            }
        }

        private void SetNext()
        {
            index++;

            if (data.Count > index)
            {
                text.SetText(data[index].Text);
                text.DOFade(1, 0.3f).SetEase(Ease.Linear).OnComplete(() => acceptClick = true);
            }
            else
            {
                tutorialBox.DOFade(0, 0.3f).SetEase(Ease.Linear).OnComplete(FinishTutorial);
            }
        }

        private void FinishTutorial()
        {
            tutorialBox.blocksRaycasts = false;
            Room.GameState.EndTutorial();
        }

        public void ShowNext()
        {
            if (acceptClick)
            {
                acceptClick = false;
                text.DOFade(0, 0.3f).SetEase(Ease.Linear).OnComplete(SetNext);
            }
        }

        public void StartTutorial()
        {
            tutorialBox.blocksRaycasts = true;
            tutorialBox.DOFade(1, 0.3f).SetEase(Ease.Linear).OnComplete(SetNext);
        }
    }
}