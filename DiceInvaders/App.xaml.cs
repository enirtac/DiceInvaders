using System.Windows;
using DiceInvaders.ViewModels;

namespace DiceInvaders
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var window = new MainWindow();
            var viewModel = new GameViewModel();
            window.DataContext = viewModel;
            window.Show();
        }
    }
}
