namespace MealCompensationCalculator.WPF.ViewModels
{
    public class MealCompensationViewModel : ViewModelBase
    {
        private string _compensationName;
        public string CompensationName
        {
            get
            {
                return _compensationName;
            }
            set
            {
                _compensationName = value;
                OnPropertyChanged(nameof(CompensationName));
            }
        }

        private decimal _compensationAmount;
        public decimal CompensationAmount
        {
            get
            {
                return _compensationAmount;
            }
            set
            {
                _compensationAmount = value;
                OnPropertyChanged(nameof(CompensationAmount));
            }
        }

        private string _startTimeCompensation;
        public string StartTimeCompensation
        {
            get
            {
                return _startTimeCompensation;
            }
            set
            {
                _startTimeCompensation = value;
                OnPropertyChanged(nameof(StartTimeCompensation));
            }
        }

        private string _endTimeCompensation;
        public string EndTimeCompensation
        {
            get
            {
                return _endTimeCompensation;
            }
            set
            {
                _endTimeCompensation = value;
                OnPropertyChanged(nameof(EndTimeCompensation));
            }
        }
    }
}