using System.Threading.Tasks;
using MealCompensationCalculator.WPF.Stores;

namespace MealCompensationCalculator.WPF.Commands
{
    public class LoadConfigCommand : AsyncCommandBase
    {
        private readonly ConfigStore _configStore;

        public LoadConfigCommand(ConfigStore configStore)
        {
            _configStore = configStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            await _configStore.Load();
        }
    }
}