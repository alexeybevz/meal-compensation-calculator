using System;
using System.Threading.Tasks;
using MealCompensationCalculator.Domain.Commands;
using MealCompensationCalculator.Domain.Models;
using MealCompensationCalculator.Domain.Queries;

namespace MealCompensationCalculator.WPF.Stores
{
    public class ConfigStore
    {
        private readonly IGetConfigQuery _getConfigQuery;
        private readonly ISaveConfigCommand _saveConfigCommand;

        public Config Config { get; set; }

        public event Action ConfigLoaded;
        public event Action ConfigSaved;

        public ConfigStore(IGetConfigQuery getConfigQuery, ISaveConfigCommand saveConfigCommand)
        {
            _getConfigQuery = getConfigQuery;
            _saveConfigCommand = saveConfigCommand;
        }

        public async Task Load()
        {
            Config = await _getConfigQuery.Execute();
            ConfigLoaded?.Invoke();
        }

        public async Task Save(Config config)
        {
            await _saveConfigCommand.Execute(config);
            ConfigSaved?.Invoke();
        }
    }
}