namespace DiceInvaders.Models
{
    public class Game 
    {
        public int CanvasHeight { get; set; }
        public int CanvasWidth { get; set; }
        public bool IsRunning { get; set; }
        public int Speed { get; set; }
        public int Level { get; set; }
        public int BombDropChance { get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }
        public int Life{get; set;}
        public int Score{get; set;}
        public string GameInformation { get; set; }
    }
}
