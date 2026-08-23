using ArdJam2026.Gameplay;
using ArdJam2026.MainMenu;
using SaintsField;
using SaintsField.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
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
        public Scene? CurrentScene { get; protected set; }

        public GameInstance GameInstance { get; }

        protected GameStateBase(GameInstance instance)
        {
            GameInstance = instance;
        }

        public abstract void Start();
        public abstract void Stop();

        public void SceneLoaded(Scene scene)
        {
            CurrentScene = scene;
            OnSceneLoaded();
        }
        protected abstract void OnSceneLoaded();
        public void SceneUnloaded(Scene scene)
        {
            if (CurrentScene == scene)
            {
                CurrentScene = null;
                OnSceneUnloaded();
            }
        }
        protected virtual void OnSceneUnloaded() { }
    }

    public class GameInstance
    {
        private readonly Dictionary<GameStateType, GameStateBase> gameStates = new();
        private readonly Dictionary<string, GameStateType> expectedGameStateByScene = new();
        private GameStateType currentState = GameStateType.Bootstrap;
        private LevelConfig loadingLevel;

        public GameStateBase CurrentGameState => gameStates[currentState];

        private readonly GameInstanceHelper helper;

        public GameConfiguration Configuration { get; }
        public LevelConfig CurrentLevel { get; private set; }

        public GameInstance(GameConfiguration configuration, GameInstanceHelper helper)
        {
            this.helper = helper;
            helper.Initialize(this);

            Configuration = configuration;
            Debug.Assert(configuration, "Configuration not loaded.");

            gameStates[GameStateType.Bootstrap] = new BootstrapGameState(this);
            gameStates[GameStateType.Menu] = new MenuGameState(this);
            gameStates[GameStateType.Gameplay] = new GameplayGameState(this);

            if (!string.IsNullOrEmpty(configuration.MenuScene))
                expectedGameStateByScene[configuration.MenuScene] = GameStateType.Menu;
            foreach (LevelConfig level in configuration.Levels)
            {
                if (!string.IsNullOrEmpty(level.Scene))
                    expectedGameStateByScene[level.Scene] = GameStateType.Gameplay;
            }

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        }

        private void SceneManager_sceneUnloaded(Scene scene)
        {
            CurrentGameState.SceneUnloaded(scene);
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
        {
            string path = RuntimeUtil.TrimScenePath(scene.path, true);

            CurrentLevel = null;
            if (loadingLevel && loadingLevel.Scene == path)
            {
                CurrentLevel = loadingLevel;
            }

            if (!expectedGameStateByScene.TryGetValue(path, out GameStateType expectedGameState))
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
            EventSystem eventSystem = GameObject.Instantiate(Configuration.EventSystem);
            Debug.Assert(eventSystem, "Could not create event system");
            eventSystem.name = "EventSystem";
            GameObject.DontDestroyOnLoad(eventSystem.gameObject);

            InputSystem.actions.Enable();

#if UNITY_EDITOR
            // Shorthand for the editor, so we automatically start into the game directly
            if (GameObject.FindAnyObjectByType<Room>())
            {
                // Store, in case we want to restart, so the correct game state is loaded
                expectedGameStateByScene[SceneManager.GetActiveScene().path] = GameStateType.Gameplay;
                SetGameStateForScene(SceneManager.GetActiveScene(), GameStateType.Gameplay);
                return;
            }
#endif

            CurrentGameState.Start();
            CurrentGameState.SceneLoaded(SceneManager.GetActiveScene());
        }

        public void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        public void LoadScene(Scene scene)
        {
            SceneManager.LoadScene(scene.path);
        }

        public void LoadLevel(LevelConfig level)
        {
            loadingLevel = level;
            SceneManager.LoadScene(level.Scene);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Main()
        {
            GameConfiguration configuration = Resources.Load<GameConfiguration>("GameConfig");

            GameObject gameInstanceContainer = new("Game Instance");
            GameInstanceHelper helper = gameInstanceContainer.AddComponent<GameInstanceHelper>();
            GameObject.DontDestroyOnLoad(gameInstanceContainer);

            GameInstance gameInstance = new(configuration, helper);
            gameInstance.StartGame();
        }

        public void StopGame()
        {
            CurrentGameState.Stop();
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
            SceneManager.sceneUnloaded -= SceneManager_sceneUnloaded;
            GameObject.Destroy(helper);
        }
    }
}
