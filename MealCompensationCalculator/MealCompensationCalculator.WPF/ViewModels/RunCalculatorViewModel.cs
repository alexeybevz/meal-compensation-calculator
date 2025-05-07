using System.Windows.Input;
using MealCompensationCalculator.WPF.Commands;

namespace MealCompensationCalculator.WPF.ViewModels
{
    public class RunCalculatorViewModel : ViewModelBase
    {
        private bool _isNotificationVisible;
        public bool IsNotificationVisible
        {
            get
            {
                return _isNotificationVisible;
            }
            set
            {
                _isNotificationVisible = value;
                OnPropertyChanged(nameof(IsNotificationVisible));
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public ICommand ExecuteMealCompensationCalculatorCommand { get; }

        public RunCalculatorViewModel(ConfigViewModel configViewModel)
        {
            ExecuteMealCompensationCalculatorCommand = new ExecuteMealCompensationCalculatorCommand(this, configViewModel);
        }
    }
}