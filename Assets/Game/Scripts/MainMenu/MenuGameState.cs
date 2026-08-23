using DG.Tweening;
using SaintsField;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArdJam2026.MainMenu
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class MainMenuBase : MonoBehaviour
    {
        [SerializeField]
        [GetComponent]
        private CanvasGroup mainCanvas;

        public MenuGameState GameState { get; private set; }
        public bool IsVisible { get; private set; }

        public void Initialize(MenuGameState gameState)
        {
            GameState = gameState;
            IsVisible = false;
            gameObject.SetActive(true);

            mainCanvas.alpha = 0;
            mainCanvas.blocksRaycasts = false;

            OnInitialize();
        }

        protected virtual void OnInitialize()
        {

        }

        public void Show()
        {
            if (!IsVisible)
            {
                IsVisible = true;
                mainCanvas.blocksRaycasts = true;
                mainCanvas.DOFade(1, 0.3f);
            }
        }

        public void Hide()
        {
            if (IsVisible)
            {
                IsVisible = false;
                mainCanvas.blocksRaycasts = false;
                mainCanvas.DOFade(0, 0.3f);
            }
        }
    }

    public class MenuGameState : GameStateBase
    {
        private MainMenu mainMenu;
        private Credits credits;
        private LevelSelect levelSelect;

        private readonly List<MainMenuBase> menus = new();

        public MenuGameState(GameInstance instance) : base(instance)
        {
        }

        protected override void OnSceneLoaded()
        {
            mainMenu = GameObject.FindAnyObjectByType<MainMenu>(FindObjectsInactive.Include);
            Debug.Assert(mainMenu, "Couldn't find main menu");
            credits = GameObject.FindAnyObjectByType<Credits>(FindObjectsInactive.Include);
            Debug.Assert(credits, "Couldn't find credits");
            levelSelect = GameObject.FindAnyObjectByType<LevelSelect>(FindObjectsInactive.Include);
            Debug.Assert(levelSelect, "Couldn't find level select");
            menus.Clear();
            menus.Add(mainMenu);
            menus.Add(credits);
            menus.Add(levelSelect);

            foreach (MainMenuBase menu in menus)
            {
                menu.Initialize(this);
            }

            Show(mainMenu);
        }

        public override void Start()
        {
        }

        public override void Stop()
        {
            menus.Clear();
        }

        public void StartGame(LevelConfig level = null)
        {
            if (!level)
            {
                level = GameInstance.Configuration.Levels.FirstOrDefault();
            }
            Debug.Assert(level, "No level found.");
            if (level)
            {
                GameInstance.LoadLevel(level);
            }
        }

        public void ShowMainMenu()
        {
            Show(mainMenu);
        }

        public void ShowLevelSelect()
        {
            Show(levelSelect);
        }

        public void ShowCredits()
        {
            Show(credits);
        }

        private void Show(MainMenuBase target)
        {
            if (target.IsVisible)
                return;

            foreach (MainMenuBase menu in menus)
            {
                if (menu != target)
                {
                    menu.Hide();
                }
            }

            target.Show();
        }
    }
}