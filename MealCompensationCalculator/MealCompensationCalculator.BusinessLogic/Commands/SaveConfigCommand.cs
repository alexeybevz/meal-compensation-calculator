using System.Configuration;
using System.Globalization;
using System.Threading.Tasks;
using MealCompensationCalculator.Domain.Commands;
using MealCompensationCalculator.Domain.Models;

namespace MealCompensationCalculator.BusinessLogic.Commands
{
    public class SaveConfigCommand : ISaveConfigCommand
    {
        public async Task Execute(Config config)
        {
            await Task.Run(() =>
            {
                var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configuration.AppSettings.Settings;

                settings["StartTimeDayCompensation"].Value = config.DayCompensation.StartTimeCompensation.ToString();
                settings["EndTimeDayCompensation"].Value = config.DayCompensation.EndTimeCompensation.ToString();

                settings["StartTimeDayEveningCompensation"].Value = config.DayEveningCompensation.StartTimeCompensation.ToString();
                settings["EndTimeDayEveningCompensation"].Value = config.DayEveningCompensation.EndTimeCompensation.ToString();

                settings["DayCompensationValue"].Value = config.DayCompensation.Compensation.ToString(CultureInfo.InvariantCulture);
                settings["DayEveningCompensationValue"].Value = config.DayEveningCompensation.Compensation.ToString(CultureInfo.InvariantCulture);

                settings["PathToSaveReports"].Value = config.PathToSaveReports;

                configuration.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            });
        }
    }
}