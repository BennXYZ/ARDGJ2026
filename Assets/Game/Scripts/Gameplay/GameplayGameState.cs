using ArdJam2026.Gameplay.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

        // TODO: Cleanup reference when changing level
        private GameplayHud hud;

        public GameplayGameState(GameInstance gameInstance) : base(gameInstance)
        {
        }

        public override void SceneLoaded(Scene scene)
        {
            CurrentCamera = Camera.main;
            Debug.Assert(CurrentCamera, "Cannot find camera.");
            CurrentRoom = GameObject.FindAnyObjectByType<Room>();
            Debug.Assert(CurrentRoom, $"Could not find a room in the scene {scene.path}");
            if (!CurrentRoom)
            {
                GameInstance.LoadScene(GameInstance.Configuration.MenuScene);
                return;
            }

            CurrentRoom.Initialize(this);

            CurrentState = State.Running;

            hud = GameObject.Instantiate<GameplayHud>(GameInstance.Configuration.GameplayHud, new InstantiateParameters() { scene = scene });
            hud.Initialize(this);
            hud.Show();
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

            // TODO: Show Pause Menu
        }

        public void Unpause()
        {
            if (CurrentState != State.Paused)
                return;

            CurrentState = State.Running;
            hud.Show();

            // TODO: Hide Pause Menu
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
        }
    }
}