namespace MealCompensationCalculator.WPF.ViewModels
{
    public class MealCompensationCalculatorViewModel : ViewModelBase
    {
        public static MealCompensationCalculatorViewModel LoadViewModel()
        {
            var vm = new MealCompensationCalculatorViewModel();
            return vm;
        }
    }
}