using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ArdJam2026.Gameplay
{
    public class GameState
    {
        private readonly GameInstance gameInstance;
        private readonly PlayerController component;

        public Room CurrentRoom { get; private set; }
        public Camera CurrentCamera { get; private set; }

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
        }

        public void DoInteractAction()
        {
            if (CurrentRoom)
            {
                CurrentRoom.PerformInteract();
            }
        }

        public void DoPossessAction()
        {
            if (CurrentRoom && CurrentCamera)
            {
                Vector3 worldPoint = CurrentCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                CurrentRoom.Possess(worldPoint);
            }
        }

        public void DoMoveAction(Vector2Int direction)
        {
            if (CurrentRoom)
            {
                CurrentRoom.PerformMove(direction);
            }
        }
    }
}