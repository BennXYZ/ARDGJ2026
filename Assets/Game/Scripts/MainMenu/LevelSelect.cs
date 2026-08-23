using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArdJam2026.MainMenu
{
    public class LevelSelect : MainMenuBase
    {
        [SerializeField]
        private LevelButton buttonPrefab;

        [SerializeField]
        private int maxLevels = 8;

        [SerializeField]
        private RectTransform buttonContainer;

        [SerializeField]
        private Button nextButton;

        [SerializeField]
        private Button previousButton;

        private int offset;

        public IReadOnlyList<LevelConfig> Levels => GameState.GameInstance.Configuration.Levels;
        private readonly List<LevelButton> buttons = new();

        protected override void OnInitialize()
        {
            for (int i = 0; i < maxLevels; i++)
            {
                LevelButton button = Instantiate(buttonPrefab, buttonContainer);
                button.Initialize(this);
                buttons.Add(button);
            }

            LoadLevels();
        }

        private void LoadLevels()
        {
            previousButton.interactable = offset > 0;
            nextButton.interactable = offset + maxLevels < Levels.Count;

            for (int i = 0; i < maxLevels; i++)
            {
                int levelIndex = i + offset;
                LevelConfig config = Levels.Count > levelIndex ? Levels[levelIndex] : null;
                buttons[i].SetLevel(config);
            }
        }

        public void BackToMenu()
        {
            GameState.ShowMainMenu();
        }

        public void StartLevel(LevelConfig level)
        {
            GameState.StartGame(level);
        }

        public void NextPage()
        {
            offset += maxLevels;
            LoadLevels();
        }

        public void PreviousPage()
        {
            offset = Math.Max(offset - maxLevels, 0);
            LoadLevels();
        }
    }
}