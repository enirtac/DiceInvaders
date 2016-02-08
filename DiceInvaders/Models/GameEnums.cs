
namespace DiceInvaders.Models
{
    public enum GameObjectType
    {
        Player,
        Rocket,
        Alien,
        Bomb
    }
    //Every movmentstep move a GameObject 15px
    public enum Direction
    {
        Left=-15,
        Right = 15,
        Up = -30,
        Down = 30
  
    }
}
