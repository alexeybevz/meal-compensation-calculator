namespace MealCompensationCalculator.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MealCompensationCalculatorViewModel MealCompensationCalculatorViewModel { get; }

        public MainViewModel()
        {
            MealCompensationCalculatorViewModel = MealCompensationCalculatorViewModel.LoadViewModel();
        }
    }
}