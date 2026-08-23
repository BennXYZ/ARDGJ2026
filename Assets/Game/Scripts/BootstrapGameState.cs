namespace ArdJam2026
{
    public class BootstrapGameState : GameStateBase
    {
        public BootstrapGameState(GameInstance instance) : base(instance)
        {
        }

        protected override void OnSceneLoaded()
        {
            GameInstance.LoadScene(GameInstance.Configuration.MenuScene);
        }

        public override void Start()
        {

        }

        public override void Stop()
        {

        }
    }
}