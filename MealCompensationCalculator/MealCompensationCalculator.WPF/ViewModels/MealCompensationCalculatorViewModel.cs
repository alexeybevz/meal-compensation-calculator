using System.Windows.Input;
using MealCompensationCalculator.WPF.Commands;
using MealCompensationCalculator.WPF.Stores;

namespace MealCompensationCalculator.WPF.ViewModels
{
    public class MealCompensationCalculatorViewModel : ViewModelBase
    {
        private MealCompensationCalculatorViewModel(ConfigStore configStore)
        {
            LoadConfigCommand = new LoadConfigCommand(configStore);
        }

        public ICommand LoadConfigCommand;

        public static MealCompensationCalculatorViewModel LoadViewModel(ConfigStore configStore)
        {
            var vm = new MealCompensationCalculatorViewModel(configStore);
            vm.LoadConfigCommand?.Execute(null);
            return vm;
        }
    }
}