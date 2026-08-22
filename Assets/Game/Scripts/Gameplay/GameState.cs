using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ArdJam2026.Gameplay
{
    public class GameState
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

        private readonly GameInstance gameInstance;
        private readonly PlayerController component;

        public Room CurrentRoom { get; private set; }
        public Camera CurrentCamera { get; private set; }

        public State CurrentState { get; private set; } = State.Paused;

        public GameState(GameInstance gameInstance)
        {
            this.gameInstance = gameInstance;

            GameObject gameObject = new("GameState");
            component = gameObject.AddComponent<PlayerController>();
            component.Initialize(this);
            GameObject.DontDestroyOnLoad(gameObject);
        }

        public void SceneLoaded(Scene scene)
        {
            CurrentCamera = Camera.main;
            Debug.Assert(CurrentCamera, "Cannot find camera.");
            CurrentRoom = GameObject.FindAnyObjectByType<Room>();
            CurrentRoom.Initialize(this);

            CurrentState = State.Running;
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
    }
}