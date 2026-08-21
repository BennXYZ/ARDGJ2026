using ArdJam2026.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ArdJam2026
{
    public class GameInstance
    {
        public GameState GameState { get; }

        public GameConfiguration Configuration { get; }

        public GameInstance(GameConfiguration configuration)
        {
            Configuration = configuration;
            Debug.Assert(configuration, "Configuration not loaded.");

            GameState = new(this);

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            GameState.SceneLoaded(SceneManager.GetActiveScene());
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GameState.SceneLoaded(scene);
        }

        private void StartGame()
        {
            // TODO: Initialize Game
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
