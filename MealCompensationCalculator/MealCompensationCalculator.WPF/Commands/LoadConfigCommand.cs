using System;
using System.Threading.Tasks;
using MealCompensationCalculator.WPF.Stores;
using MealCompensationCalculator.WPF.ViewModels;

namespace MealCompensationCalculator.WPF.Commands
{
    public class LoadConfigCommand : AsyncCommandBase
    {
        private readonly MealCompensationCalculatorViewModel _mealCompensationCalculatorViewModel;
        private readonly ConfigStore _configStore;

        public LoadConfigCommand(MealCompensationCalculatorViewModel mealCompensationCalculatorViewModel,
            ConfigStore configStore)
        {
            _mealCompensationCalculatorViewModel = mealCompensationCalculatorViewModel;
            _configStore = configStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _mealCompensationCalculatorViewModel.ErrorMessage = null;
            _mealCompensationCalculatorViewModel.IsLoading = true;

            try
            {
                await _configStore.Load();
            }
            catch (Exception ex)
            {
                _mealCompensationCalculatorViewModel.ErrorMessage = "Возникла ошибка при чтении app.config файла.";
            }
            finally
            {
                _mealCompensationCalculatorViewModel.IsLoading = false;
            }
        }
    }
}