namespace ArdJam2026.Gameplay
{
    public class Goal : FloorButton
    {
        // Just to make the naming less weird when reading the code accessing the goal
        public bool Reached => IsPressed;
    }
}