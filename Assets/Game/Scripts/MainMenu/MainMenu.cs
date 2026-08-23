namespace ArdJam2026.MainMenu
{
    public class MainMenu : MainMenuBase
    {
        public void StartGame()
        {
            GameState.StartGame();
        }

        public void LevelSelect()
        {
            GameState.ShowLevelSelect();
        }

        public void Credits()
        {
            GameState.ShowCredits();
        }
    }
}