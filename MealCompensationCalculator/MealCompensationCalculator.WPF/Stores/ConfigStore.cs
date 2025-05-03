using System;
using System.Threading.Tasks;
using MealCompensationCalculator.Domain.Models;
using MealCompensationCalculator.Domain.Queries;

namespace MealCompensationCalculator.WPF.Stores
{
    public class ConfigStore
    {
        private readonly IGetConfigQuery _getConfigQuery;
        private Config _config;

        public Config Config => _config;

        public event Action ConfigLoaded;

        public ConfigStore(IGetConfigQuery getConfigQuery)
        {
            _getConfigQuery = getConfigQuery;
        }

        public async Task Load()
        {
            _config = await _getConfigQuery.Execute();
            ConfigLoaded?.Invoke();
        }
    }
}