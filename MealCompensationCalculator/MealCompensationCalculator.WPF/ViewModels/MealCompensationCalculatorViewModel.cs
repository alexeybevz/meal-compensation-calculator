using System.Windows.Input;
using MealCompensationCalculator.WPF.Commands;
using MealCompensationCalculator.WPF.Stores;

namespace MealCompensationCalculator.WPF.ViewModels
{
    public class MealCompensationCalculatorViewModel : ViewModelBase
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
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

        public ConfigViewModel ConfigViewModel { get; }
        public RunCalculatorViewModel RunCalculatorViewModel { get; }

        public ICommand LoadConfigCommand { get; }

        private MealCompensationCalculatorViewModel(ConfigStore configStore)
        {
            ConfigViewModel = new ConfigViewModel(configStore);
            RunCalculatorViewModel = new RunCalculatorViewModel(ConfigViewModel);
            LoadConfigCommand = new LoadConfigCommand(this, configStore);
        }

        public static MealCompensationCalculatorViewModel LoadViewModel(ConfigStore configStore)
        {
            var vm = new MealCompensationCalculatorViewModel(configStore);
            vm.LoadConfigCommand?.Execute(null);
            return vm;
        }
    }
}