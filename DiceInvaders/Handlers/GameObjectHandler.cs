using System.Collections.Generic;
using System.Linq;
using DiceInvaders.Models;

namespace DiceInvaders.Handlers
{
    public class GameObjectHandler
    {
        public GameObject CreatePlayer(int canvasHeight)
        {
            
            const string imageSource = @"pack://application:,,,/DiceInvaders;component/Sprites/player.png";

            var player = new GameObject()
            {
                Sprite = imageSource,
                Left = 0,
                Top = canvasHeight-60,
                Type = GameObjectType.Player,
                ImageHeight = 21,
                ImageWidth = 32
            };
            return player;
        }

        public IList<GameObject> CreateAlienArmy(int columns, int rows)
        {
            var alienList = new List<GameObject>();
            var left = 0;
            //Sets the top of the Aliens to 40 since there are labels on the top of the view
            var top = 40;

            for (var row = 0; row < rows; row++)
            {
                var imageSource = row%2 == 0
                    ? "pack://application:,,,/DiceInvaders;component/Sprites/enemy1.png"
                    : "pack://application:,,,/DiceInvaders;component/Sprites/enemy2.png";
                for (var column = 0; column < columns; column++)
                {
                    var alienObject = new GameObject
                    {
                        Sprite = imageSource,
                        Left = left,
                        Top = top,
                        Type = GameObjectType.Alien,
                        ImageWidth = 32,
                        ImageHeight = 27
                    };

                    alienList.Add(alienObject);
                    left += 32;
                }
                left = 0;
                top += 27;
            }

            return alienList.OrderBy(t => t.Left).ToList();
        }

        public GameObject CreateBomb(int left, int top)
        {
            const string imageSource = "pack://application:,,,/DiceInvaders;component/Sprites/Bomb.png";

            var bomb = new GameObject
            {
                Sprite = imageSource,
                Left = left,
                Top = top,
                Type = GameObjectType.Bomb,
                ImageHeight = 32,
                ImageWidth = 12
            };
            return bomb;
        }

        public GameObject CreateRocket(GameObject player)
        {
            const string imageSource = "pack://application:,,,/DiceInvaders;component/Sprites/rocket.png";
           
            var rocket = new GameObject
            {
                Sprite = imageSource,
                //Top and left from player, added imagewidth and height since the rocket needs to be centered over the player sprite
                Left = player.Left+player.ImageWidth/2,
                Top = player.Top-player.ImageHeight/2,
                Type = GameObjectType.Rocket,
                ImageWidth = 8,
                ImageHeight = 26

            };
            return rocket;
        }

        public IList<GameObject> MoveAllBombsAndRockets(IList<GameObject> gameObjects, int canvasHeight, int canvasWidth)
        {
            foreach (var gameObject in gameObjects.Where(x => x.Type == GameObjectType.Bomb))
            {
                gameObject.Top += (int) Direction.Down;
            }
            foreach (var gameObject in gameObjects.Where(x => x.Type == GameObjectType.Rocket))
            {
                gameObject.Top += (int) Direction.Up;
            }
            return gameObjects;
        }

        public Direction MoveAliens(IList<GameObject> listOfAliens, int canvasWidth, Direction direction)
        {
            var newDirection = direction;

            if (direction == Direction.Right)
            {
                if (listOfAliens.Last() != null && listOfAliens.Last().Left + listOfAliens.Last().ImageWidth > canvasWidth)
                {
                    newDirection = Direction.Left;
                    foreach (var alien in listOfAliens)
                    {
                        alien.Top += (int) Direction.Down;
                    }
                }
                else
                {
                    foreach (var alien in listOfAliens)
                    {
                        alien.Left += (int) direction;
                    }

                }
            }
            if (direction == Direction.Left)
            {
                if (listOfAliens.First() != null && listOfAliens.First().Left < 0)
                {
                    newDirection = Direction.Right;
                    foreach (var alien in listOfAliens)
                    {
                        alien.Top += (int) Direction.Down;
                    }

                }
                else
                {
                    foreach (var alien in listOfAliens)
                    {
                        alien.Left += (int) direction;
                    }

                }

            }
            return newDirection;
        }
    }
}
