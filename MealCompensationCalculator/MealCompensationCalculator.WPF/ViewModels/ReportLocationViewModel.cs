namespace MealCompensationCalculator.WPF.ViewModels
{
    public class ReportLocationViewModel : ViewModelBase
    {
        private string _pathToReport;
        public string PathToReport
        {
            get
            {
                return _pathToReport;
            }
            set
            {
                _pathToReport = value;
                OnPropertyChanged(nameof(PathToReport));
            }
        }
    }
}