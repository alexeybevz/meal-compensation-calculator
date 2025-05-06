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

        public ICommand ExecuteMealCompensationCalculatorCommand { get; }

        public RunCalculatorViewModel(ConfigViewModel configViewModel)
        {
            ExecuteMealCompensationCalculatorCommand = new ExecuteMealCompensationCalculatorCommand(this, configViewModel);
        }
    }
}