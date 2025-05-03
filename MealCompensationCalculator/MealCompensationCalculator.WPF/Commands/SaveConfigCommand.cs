using System;
using System.Threading.Tasks;
using MealCompensationCalculator.Domain.Models;
using MealCompensationCalculator.WPF.Stores;
using MealCompensationCalculator.WPF.ViewModels;

namespace MealCompensationCalculator.WPF.Commands
{
    public class SaveConfigCommand : AsyncCommandBase
    {
        private readonly ConfigViewModel _configViewModel;
        private readonly ConfigStore _configStore;

        public SaveConfigCommand(ConfigViewModel configViewModel, ConfigStore configStore)
        {
            _configViewModel = configViewModel;
            _configStore = configStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            var vm = _configViewModel.DayCompensationViewModel;
            var dayCompensation = new MealCompensation(vm.CompensationAmount, TimeSpan.Parse(vm.StartTimeCompensation), TimeSpan.Parse(vm.EndTimeCompensation));

            vm = _configViewModel.DayEveningCompensationViewModel;
            var dayEveningCompensation = new MealCompensation(vm.CompensationAmount, TimeSpan.Parse(vm.StartTimeCompensation), TimeSpan.Parse(vm.EndTimeCompensation));

            var pathToOutputDirectory = _configViewModel.ReportLocationViewModel.PathToReport;

            var config = new Config(pathToOutputDirectory, dayCompensation, dayEveningCompensation);

            await _configStore.Save(config);
        }
    }
}