using System.Windows.Input;
using MealCompensationCalculator.WPF.Commands;
using MealCompensationCalculator.WPF.Stores;

namespace MealCompensationCalculator.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MealCompensationCalculatorViewModel MealCompensationCalculatorViewModel { get; }

        public ICommand MyClosedCommand { get; }

        public MainViewModel(ConfigStore configStore, MealCompensationCalculatorViewModel mealCompensationCalculatorViewModel)
        {
            MealCompensationCalculatorViewModel = mealCompensationCalculatorViewModel;
            MyClosedCommand = new SaveConfigCommand(MealCompensationCalculatorViewModel.ConfigViewModel, configStore);
        }
    }
}