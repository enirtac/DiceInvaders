using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using DiceInvaders.Models;

namespace DiceInvaders.Handlers
{
    public class CollisionHandler
    {
        public ObservableCollection<GameObject> Collision(List<GameObject> attackers, List<GameObject> defenders,
            ObservableCollection<GameObject> gameObjects, int canvasHeight, out int numberOfCollisions)
        {
            var collisions = 0;

            foreach (var attacker in attackers)
            {
                var attackerRectangle = new Rect(attacker.Left, attacker.Top,
                    attacker.ImageWidth, attacker.ImageHeight);

                foreach (var defender in defenders)
                {
                    var defenderRectangle = new Rect(defender.Left, defender.Top,
                        defender.ImageWidth, defender.ImageHeight);

                    var collisionDetected = attackerRectangle.IntersectsWith(defenderRectangle);
                    if (collisionDetected)
                    {
                        gameObjects.Remove(attacker);
                        if (defender.Type != GameObjectType.Player)
                        {
                            gameObjects.Remove(defender);
                        }


                        collisions += 1;
                        break;
                    }
                }
            }


            numberOfCollisions = collisions;
            return gameObjects;
        }
    }
}