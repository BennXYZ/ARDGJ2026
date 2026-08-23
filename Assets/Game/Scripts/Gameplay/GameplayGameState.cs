using ArdJam2026.Gameplay.UI;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

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

        private GameplayHud hud;
        private PauseMenu pauseMenu;

        public GameplayGameState(GameInstance gameInstance) : base(gameInstance)
        {
        }

        protected override void OnSceneLoaded()
        {
            CurrentCamera = Camera.main;
            Debug.Assert(CurrentCamera, "Cannot find camera.");
            CurrentRoom = GameObject.FindAnyObjectByType<Room>();
            Debug.Assert(CurrentRoom, $"Could not find a room in the scene {CurrentScene?.path}");
            if (!CurrentRoom)
            {
                GameInstance.LoadScene(GameInstance.Configuration.MenuScene);
                return;
            }

            CurrentRoom.Initialize(this);

            CurrentState = State.Running;

            hud = GameObject.Instantiate<GameplayHud>(GameInstance.Configuration.GameplayHud, new InstantiateParameters() { scene = CurrentScene.Value });
            hud.Initialize(this);
            hud.Show();

            pauseMenu = GameObject.Instantiate<PauseMenu>(GameInstance.Configuration.PauseMenu, new InstantiateParameters() { scene = CurrentScene.Value });
            pauseMenu.Initialize(this);
        }

        protected override void OnSceneUnloaded()
        {
            if (hud)
            {
                GameObject.Destroy(hud.gameObject);
                hud = null;
            }
            if (pauseMenu)
            {
                GameObject.Destroy(pauseMenu.gameObject);
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
            hud.Show();

            pauseMenu.Hide();
        }

        public override void Start()
        {
            GameObject gameObject = new("GameState");
            component = gameObject.AddComponent<PlayerController>();
            component.Initialize(this);
            GameObject.DontDestroyOnLoad(gameObject);
        }

        public override void Stop()
        {
            if (component)
            {
                GameObject.Destroy(component.gameObject);
                component = null;
            }

            if (hud)
            {
                GameObject.Destroy(hud.gameObject);
                hud = null;
            }
            if (pauseMenu)
            {
                GameObject.Destroy(pauseMenu.gameObject);
                pauseMenu = null;
            }
        }

        public void Restart()
        {
            if (CurrentScene.HasValue)
                GameInstance.LoadScene(CurrentScene.Value);
        }

        public void BackToMenu()
        {
            GameInstance.LoadScene(GameInstance.Configuration.MenuScene);
        }
    }
}