using System.Windows;
using MealCompensationCalculator.WPF.Ioc;

namespace MealCompensationCalculator.WPF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IocKernel.Initialize(new IocConfiguration());

            MainWindow = IocKernel.Get<MainWindow>();
            MainWindow.Show();
        }
    }
}
