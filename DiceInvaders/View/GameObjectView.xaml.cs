using System.Linq;
using System.Windows;
using System.Windows.Input;
using DiceInvaders.Models;
using DiceInvaders.ViewModels;

namespace DiceInvaders.View
{
    public partial class GameObjectView
    {
        public GameObjectView()
        {
            InitializeComponent();
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window != null)
                window.KeyDown += HandleKeyPress;
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
             var viewModel = (GameViewModel) DataContext;

            if (viewModel.IsRunning)
            {
                if (e.Key == Key.Left)
                {
                    viewModel.MovePlayer(Direction.Left);
                }
                if (e.Key == Key.Right)
                {
                    viewModel.MovePlayer(Direction.Right);
                }
                if (e.Key == Key.Space)
                {
                    viewModel.CreateRocket();
                }
            }
            else
            {
              
                viewModel.StartGame();
            }
        }
    }
}