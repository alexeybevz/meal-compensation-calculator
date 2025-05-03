namespace MealCompensationCalculator.WPF.ViewModels
{
    public class ConfigViewModel : ViewModelBase
    {
        public MealCompensationViewModel DayCompensationViewModel { get; }
        public MealCompensationViewModel DayEveningCompensationViewModel { get; }
        public ReportLocationViewModel ReportLocationViewModel { get; }

        public ConfigViewModel()
        {
            DayCompensationViewModel = new MealCompensationViewModel();
            DayEveningCompensationViewModel = new MealCompensationViewModel();
            ReportLocationViewModel = new ReportLocationViewModel();
        }
    }
}