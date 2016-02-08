using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using DiceInvaders.Annotations;
using DiceInvaders.Handlers;
using DiceInvaders.Models;
using System.Collections.ObjectModel;


namespace DiceInvaders.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<GameObject> AllGameObjects
        {
            get { return _gameObjects; }
        }

        public int Score
        {
            get { return _game.Score; }
            set
            {
                if (_game.Score != value)
                {
                    _game.Score = value;

                    OnPropertyChanged();
                }
            }
        }

        public int Life
        {
            get { return _game.Life; }
            set
            {
                if (_game.Life != value)
                {
                    _game.Life = value;

                    OnPropertyChanged();
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                if (_game != null)
                    return _game.IsRunning;
                return false;
            }
            set
            {
                _game.IsRunning = value;
                OnPropertyChanged();
            }
        }


        public string GameInformation
        {
            get { return _game.GameInformation; }
            set
            {
                if (_game.GameInformation != value)
                {
                    _game.GameInformation = value;

                    OnPropertyChanged();
                }
            }
        }

        private readonly GameObjectHandler _gameObjectHandler;
        private readonly CollisionHandler _collisionHandler;
        private ObservableCollection<GameObject> _gameObjects;
        private DispatcherTimer _gameTimer;
        private Random _randomBombDrop;
        private Random _randomAlien;
        private Game _game;
        private Direction _direction = Direction.Right;


        public GameViewModel()
        {
            NewGame(715, 855, false, 500000, 10, 2);
            GameInformation = "Press any key to start game...";
            _gameObjectHandler = new GameObjectHandler();
            _collisionHandler = new CollisionHandler();
        }

        private void NewGame(int canvasHeight, int canvasWidth, bool running, int speed, int columns, int rows)
        {
            _game = new Game
            {
                CanvasHeight = canvasHeight,
                CanvasWidth = canvasWidth,
                Rows = rows,
                Columns = columns,
                IsRunning = running,
                Speed = speed,
                BombDropChance = 3,
                Level = 1,
                Life = 3,
                Score = 0
            };

            _gameObjects = new ObservableCollection<GameObject>();
        }

        public void StartGame()
        {
            _game.IsRunning = true;
            _gameTimer = new DispatcherTimer();
            _gameTimer.Tick += GameTimer_Tick;
            _gameTimer.Interval = new TimeSpan(_game.Speed);
            _gameTimer.Start();
            GameInformation = string.Empty;
            Score = 0;
            Life = 3;
            _randomBombDrop = new Random();
            _randomAlien = new Random();


            _gameObjects.Add(_gameObjectHandler.CreatePlayer(_game.CanvasHeight));
            CreateAlienArmy();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (AliensHitBottom())
            {
                KillGame();
            }
            CheckForCollisions();
            RemoveRockets();
            if (Life <= 0)
            {
                KillGame();
                return;
            }

            _gameObjectHandler.MoveAllBombsAndRockets(_gameObjects, _game.CanvasHeight, _game.CanvasWidth);

            var aliensAlive = _gameObjects.Where(x => x.Type == GameObjectType.Alien).ToList();
            if (aliensAlive.Count == 0)
            {
                IncreaseLevel();
                CreateAlienArmy();
            }

            var randomNumber = _randomBombDrop.Next(1, 100);
            if (randomNumber <= _game.BombDropChance)
            {
                CreateBomb();
            }

            _direction = _gameObjectHandler.MoveAliens(
                _gameObjects.Where(t => t.Type == GameObjectType.Alien).ToList(), _game.CanvasWidth, _direction);
        }

        public void CreateRocket()
        {
            var player = _gameObjects.FirstOrDefault(t => t.Type == GameObjectType.Player);
            if (player != null)
            {
                var rocket = _gameObjectHandler.CreateRocket(player);
                _gameObjects.Add(rocket);
            }
        }

        public void MovePlayer(Direction direction)
        {
            var player = _gameObjects.FirstOrDefault(t => t.Type == GameObjectType.Player);
            if (player != null)
            {
                if (direction == Direction.Left && player.Left > 0)
                    player.Left += (int) direction;
                if (direction == Direction.Right && player.Left + 15 + player.ImageWidth < _game.CanvasWidth)
                    player.Left += (int) direction;
            }
        }

        private void CreateBomb()
        {
            var aliensList = _gameObjects.Where(x => x.Type == GameObjectType.Alien).ToList();
            var randomAlienIndex = _randomAlien.Next(aliensList.Count);
            var bombObject = _gameObjectHandler.CreateBomb(aliensList[randomAlienIndex].Left,
                aliensList[randomAlienIndex].Top);
            _gameObjects.Add(bombObject);
        }

        private void KillGame()
        {
            Life = 0;
            _gameObjects.Clear();
            _game.IsRunning = false;
            _gameTimer.Stop();
            _game.Level = 1;
            _game.Speed = 500000;
            _game.Rows = 2;
            _game.Columns = 10;
            GameInformation = ("GAME OVER! \nPress any key to play again!");
        }

        private void CheckForCollisions()
        {
            var numberOfCollisions = 0;
            _gameObjects = _collisionHandler.Collision(
                _gameObjects.Where(x => x.Type == GameObjectType.Rocket).ToList(),
                _gameObjects.Where(x => x.Type == GameObjectType.Alien).ToList(), _gameObjects, _game.CanvasHeight,
                out numberOfCollisions);
            Score += numberOfCollisions*10;
            _gameObjects = _collisionHandler.Collision(_gameObjects.Where(x => x.Type == GameObjectType.Bomb).ToList(),
                _gameObjects.Where(x => x.Type == GameObjectType.Player).ToList(), _gameObjects, _game.CanvasHeight,
                out numberOfCollisions);
            Life -= numberOfCollisions * 1;
            _gameObjects= _collisionHandler.Collision(_gameObjects.Where(x => x.Type == GameObjectType.Alien).ToList(),
                _gameObjects.Where(x => x.Type == GameObjectType.Player).ToList(), _gameObjects, _game.CanvasHeight,
                out numberOfCollisions);
            Life -= numberOfCollisions*1;
        }

        private void IncreaseLevel()
        {
            if (_game.Level < 10)
            {
                _game.Columns++;
                _game.Rows++;
            }
            _game.Level++;
            _game.Speed -= _game.Level*10000;
            _game.BombDropChance++;
        }

        private void CreateAlienArmy()
        {
            var alienList = _gameObjectHandler.CreateAlienArmy(_game.Columns, _game.Rows);

            foreach (var alien in alienList)
            {
                _gameObjects.Add(alien);
            }
        }

        private void RemoveRockets()
        {
            var rockets = _gameObjects.Where(x => x.Type == GameObjectType.Rocket && x.Top < 0).ToList();
            foreach (var rocket in rockets)
            {
                _gameObjects.Remove(rocket);
            }
        }

        private bool AliensHitBottom()
        {
            return
                _gameObjects.Any(
                    x => x.Type == GameObjectType.Alien && x.Top + x.ImageHeight > _game.CanvasHeight - x.ImageHeight);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}