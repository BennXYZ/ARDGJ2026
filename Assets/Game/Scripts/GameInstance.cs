using ArdJam2026.Gameplay;
using SaintsField;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ArdJam2026
{
    public enum GameStateType
    {
        Bootstrap,
        Menu,
        Gameplay
    }

    public abstract class GameStateBase
    {
        public GameInstance GameInstance { get; }

        protected GameStateBase(GameInstance instance)
        {
            GameInstance = instance;
        }

        public abstract void Start();
        public abstract void Stop();

        public abstract void SceneLoaded(Scene scene);
    }

    public class GameInstance
    {
        private readonly Dictionary<GameStateType, GameStateBase> gameStates = new();
        private readonly Dictionary<string, GameStateType> expectedGameStateByScene = new();
        private GameStateType currentState = GameStateType.Bootstrap;

        public GameStateBase CurrentGameState => gameStates[currentState];
        public GameConfiguration Configuration { get; }

        public GameInstance(GameConfiguration configuration)
        {
            Configuration = configuration;
            Debug.Assert(configuration, "Configuration not loaded.");

            gameStates[GameStateType.Bootstrap] = new BootstrapGameState(this);
            gameStates[GameStateType.Menu] = new MenuGameState(this);
            gameStates[GameStateType.Gameplay] = new GameplayGameState(this);

            if (!string.IsNullOrEmpty(configuration.MenuScene.path))
                expectedGameStateByScene[configuration.MenuScene.path] = GameStateType.Menu;
            foreach (SceneReference sceneReference in configuration.Levels)
            {
            if (!string.IsNullOrEmpty(sceneReference.path))
                expectedGameStateByScene[sceneReference.path] = GameStateType.Gameplay;
            }

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!expectedGameStateByScene.TryGetValue(scene.path, out GameStateType expectedGameState))
            {
                Debug.LogError("We loaded an unexpected scene.");
            }

            SetGameStateForScene(scene, expectedGameState);
        }

        private void SetGameStateForScene(Scene scene, GameStateType gameState)
        {
            if (currentState != gameState)
            {
                CurrentGameState.Stop();
                currentState = gameState;
                CurrentGameState.Start();
            }
            CurrentGameState.SceneLoaded(scene);
        }

        private void StartGame()
        {
            InputSystem.actions.Enable();

#if UNITY_EDITOR
            // Shorthand for the editor, so we automatically start into the game directly
            if (GameObject.FindAnyObjectByType<Room>())
            {
                SetGameStateForScene(SceneManager.GetActiveScene(), GameStateType.Gameplay);
                return;
            }
#endif

            CurrentGameState.Start();
            CurrentGameState.SceneLoaded(SceneManager.GetActiveScene());
        }

        public void LoadScene(SceneReference scene)
        {
            SceneManager.LoadScene(scene);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Main()
        {
            GameConfiguration configuration = Resources.Load<GameConfiguration>("GameConfig");

            GameInstance gameInstance = new(configuration);
            gameInstance.StartGame();
        }
    }
}
