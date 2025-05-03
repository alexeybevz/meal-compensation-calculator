using System;
using System.Threading.Tasks;
using MealCompensationCalculator.BusinessLogic.Commands;
using MealCompensationCalculator.Domain.Models;
using MealCompensationCalculator.Domain.Queries;

namespace MealCompensationCalculator.WPF.Stores
{
    public class ConfigStore
    {
        private readonly IGetConfigQuery _getConfigQuery;
        private readonly SaveConfigCommand _saveConfigCommand;
        private Config _config;

        public Config Config => _config;

        public event Action ConfigLoaded;
        public event Action ConfigSaved;

        public ConfigStore(IGetConfigQuery getConfigQuery, SaveConfigCommand saveConfigCommand)
        {
            _getConfigQuery = getConfigQuery;
            _saveConfigCommand = saveConfigCommand;
        }

        public async Task Load()
        {
            _config = await _getConfigQuery.Execute();
            ConfigLoaded?.Invoke();
        }

        public async Task Save(Config config)
        {
            await _saveConfigCommand.Execute(config);
            ConfigSaved?.Invoke();
        }
    }
}