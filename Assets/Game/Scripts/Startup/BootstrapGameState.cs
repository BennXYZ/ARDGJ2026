using UnityEngine;

namespace ArdJam2026.Startup
{
    public class BootstrapGameState : GameStateBase
    {
        private IntroCutscene introCutscene;

        public BootstrapGameState(GameInstance instance) : base(instance)
        {
        }

        protected override void OnSceneLoaded()
        {
            introCutscene = GameObject.FindAnyObjectByType<IntroCutscene>();

            // Skip directly to the menu
            if (!introCutscene)
            {
                GoToMainMenu();
                return;
            }

            introCutscene.Initialize(this);
        }

        public override void Start()
        {

        }

        public override void Stop()
        {

        }

        public void GoToMainMenu()
        {
            GameInstance.LoadScene(GameInstance.Configuration.MenuScene);
        }
    }
}