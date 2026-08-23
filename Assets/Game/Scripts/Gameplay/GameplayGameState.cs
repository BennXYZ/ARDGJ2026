using ArdJam2026.Gameplay.UI;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace ArdJam2026.Gameplay
{
    public class GameplayGameState : GameStateBase
    {
        public enum State
        {
            Paused,
            Running,
            GameOver,
            GameWon
        }

        public enum GameOverReason
        {
            Win,
            Death
        }

        private PlayerController component;

        public Room CurrentRoom { get; private set; }
        public Camera CurrentCamera { get; private set; }

        public State CurrentState { get; private set; } = State.Paused;
        public LevelConfig CurrentLevel => GameInstance.CurrentLevel;
        public string CurrentLevelTitle => CurrentLevel ? CurrentLevel.Title : "Nemo";

        private GameplayHud hud;
        private PauseMenu pauseMenu;
        private GameOverHud gameLostHud;
        private GameOverHud gameWonHud;

        public GameplayGameState(GameInstance gameInstance) : base(gameInstance)
        {
        }

        protected override void OnSceneLoaded()
        {
            CurrentCamera = Camera.main;
            Debug.Assert(CurrentCamera, "Cannot find camera.");
            CurrentRoom = Object.FindAnyObjectByType<Room>();
            Debug.Assert(CurrentRoom, $"Could not find a room in the scene {CurrentScene?.path}");
            if (!CurrentRoom)
            {
                GameInstance.LoadScene(GameInstance.Configuration.MenuScene);
                return;
            }

            CurrentRoom.Initialize(this);

            CurrentState = State.Running;

            hud = Object.Instantiate(GameInstance.Configuration.GameplayHud, new InstantiateParameters() { scene = CurrentScene.Value });
            hud.Initialize(this);
            hud.Show(true);

            pauseMenu = Object.Instantiate(GameInstance.Configuration.PauseMenu, new InstantiateParameters() { scene = CurrentScene.Value });
            pauseMenu.Initialize(this);

            gameLostHud = Object.Instantiate(GameInstance.Configuration.GameLostHud, new InstantiateParameters() { scene = CurrentScene.Value });
            gameLostHud.Initialize(this);

            gameWonHud = Object.Instantiate(GameInstance.Configuration.GameWonHud, new InstantiateParameters() { scene = CurrentScene.Value });
            gameWonHud.Initialize(this);
        }

        protected override void OnSceneUnloaded()
        {
            if (hud)
            {
                Object.Destroy(hud.gameObject);
                hud = null;
            }
            if (pauseMenu)
            {
                Object.Destroy(pauseMenu.gameObject);
                pauseMenu = null;
            }
        }

        public void DoInteractAction()
        {
            if (CurrentRoom && CurrentState == State.Running)
            {
                CurrentRoom.PerformInteract();
            }
        }

        public void DoPossessAction()
        {
            if (CurrentRoom && CurrentCamera && CurrentState == State.Running)
            {
                Vector3 worldPoint = CurrentCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                CurrentRoom.Possess(worldPoint);
            }
        }

        public void DoMoveAction(Vector2Int direction)
        {
            if (CurrentRoom && CurrentState == State.Running)
            {
                CurrentRoom.PerformMove(direction);
            }
        }

        public void GameOver(GameOverReason reason)
        {
            CurrentState = reason == GameOverReason.Win ? State.GameWon : State.GameOver;

            GameOverHud hudToShow = CurrentState switch
            {
                State.GameWon => gameWonHud,
                _ => gameLostHud
            };
            hudToShow.Show();

            if (reason == GameOverReason.Death)
            {
                hud.OnDeath();
                foreach (Pawn pawn in CurrentRoom.Pawns)
                {
                    if(!pawn.IsPossessed)
                        continue;
                    pawn.Die("Death_Ground", true);
                }
            }
        }

        public void Pause()
        {
            if (CurrentState != State.Running)
                return;

            CurrentState = State.Paused;
            hud.Hide();

            pauseMenu.Show();
        }

        public void Unpause()
        {
            if (CurrentState != State.Paused)
                return;

            CurrentState = State.Running;
            hud.Show(false);

            pauseMenu.Hide();
        }

        public override void Start()
        {
            GameObject gameObject = new("GameState");
            component = gameObject.AddComponent<PlayerController>();
            component.Initialize(this);
            Object.DontDestroyOnLoad(gameObject);
        }

        public override void Stop()
        {
            if (component)
            {
                Object.Destroy(component.gameObject);
                component = null;
            }

            if (hud)
            {
                Object.Destroy(hud.gameObject);
                hud = null;
            }
            if (pauseMenu)
            {
                Object.Destroy(pauseMenu.gameObject);
                pauseMenu = null;
            }
        }

        public void Restart()
        {
            if (CurrentScene.HasValue)
                GameInstance.LoadScene(CurrentScene.Value);
        }

        public void NextLevel()
        {
            if (CurrentLevel)
            {
                int currentLevelIndex = GameInstance.Configuration.Levels.IndexOf(CurrentLevel);
                if (currentLevelIndex >= 0 && currentLevelIndex + 1 < GameInstance.Configuration.Levels.Count)
                {
                    LevelConfig nextLevel = GameInstance.Configuration.Levels[currentLevelIndex + 1];
                    GameInstance.LoadLevel(nextLevel);
                }
                else
                {
                    BackToMenu();
                }
            }
        }

        public void BackToMenu()
        {
            GameInstance.LoadScene(GameInstance.Configuration.MenuScene);
        }
    }
}