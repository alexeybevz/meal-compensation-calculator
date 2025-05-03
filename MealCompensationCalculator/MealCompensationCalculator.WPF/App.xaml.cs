using System.Windows;
using MealCompensationCalculator.BusinessLogic.Queries;
using MealCompensationCalculator.WPF.Stores;
using MealCompensationCalculator.WPF.ViewModels;

namespace MealCompensationCalculator.WPF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow = new MainWindow() { DataContext = new MainViewModel(new ConfigStore(new GetConfigQuery())) };
            MainWindow.Show();
        }
    }
}
