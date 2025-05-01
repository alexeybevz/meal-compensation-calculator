using MealCompensationCalculator.WPF.Stores;

namespace MealCompensationCalculator.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MealCompensationCalculatorViewModel MealCompensationCalculatorViewModel { get; }

        public MainViewModel(ConfigStore configStore)
        {
            MealCompensationCalculatorViewModel = MealCompensationCalculatorViewModel.LoadViewModel(configStore);
        }
    }
}