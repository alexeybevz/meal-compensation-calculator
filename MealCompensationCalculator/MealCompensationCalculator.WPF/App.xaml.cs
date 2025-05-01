using System.Windows;
using MealCompensationCalculator.WPF.Stores;
using MealCompensationCalculator.WPF.ViewModels;

namespace MealCompensationCalculator.WPF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow() { DataContext = new MainViewModel(new ConfigStore())};
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
