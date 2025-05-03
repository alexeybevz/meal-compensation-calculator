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
                ConfigurationManager.AppSettings["StartTimeDayCompensation"] = config.DayCompensation.StartTimeCompensation.ToString();
                ConfigurationManager.AppSettings["EndTimeDayCompensation"] = config.DayCompensation.EndTimeCompensation.ToString();

                ConfigurationManager.AppSettings["StartTimeDayEveningCompensation"] = config.DayEveningCompensation.StartTimeCompensation.ToString();
                ConfigurationManager.AppSettings["EndTimeDayEveningCompensation"] = config.DayEveningCompensation.EndTimeCompensation.ToString();

                ConfigurationManager.AppSettings["DayCompensationValue"] = config.DayCompensation.Compensation.ToString(CultureInfo.InvariantCulture);
                ConfigurationManager.AppSettings["DayEveningCompensationValue"] = config.DayEveningCompensation.Compensation.ToString(CultureInfo.InvariantCulture);

                ConfigurationManager.AppSettings["PathToSaveReports"] = config.PathToSaveReports;
            });
        }
    }
}