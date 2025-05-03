using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using MealCompensationCalculator.Domain.Models;
using MealCompensationCalculator.Domain.Queries;

namespace MealCompensationCalculator.BusinessLogic.Queries
{
    public class GetConfigQuery : IGetConfigQuery
    {
        public async Task<Config> Execute()
        {
            Config config = null;

            await Task.Run(() =>
            {
                var startTimeDayCompensation = ConfigurationManager.AppSettings["StartTimeDayCompensation"];
                var endTimeDayCompensation = ConfigurationManager.AppSettings["EndTimeDayCompensation"];
                var startTimeDayEveningCompensation = ConfigurationManager.AppSettings["StartTimeDayEveningCompensation"];
                var endTimeDayEveningCompensation = ConfigurationManager.AppSettings["EndTimeDayEveningCompensation"];
                var dayCompensationValue = ConfigurationManager.AppSettings["DayCompensationValue"];
                var dayEveningCompensationValue = ConfigurationManager.AppSettings["DayEveningCompensationValue"];
                var pathToSaveReportsSetting = ConfigurationManager.AppSettings["PathToSaveReports"];

                var pathToSaveReports = Directory.Exists(pathToSaveReportsSetting) ? pathToSaveReportsSetting : Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                MealCompensation dayCompensation = null;
                if (string.IsNullOrWhiteSpace(startTimeDayCompensation) || string.IsNullOrWhiteSpace(endTimeDayCompensation))
                {
                    dayCompensation = new MealCompensation(
                        0,
                        TimeSpan.Parse("00:00"),
                        TimeSpan.Parse("00:00"));
                }
                else
                {
                    decimal dayComp;
                    decimal.TryParse(dayCompensationValue, out dayComp);

                    dayCompensation = new MealCompensation(
                        decimal.Parse(dayCompensationValue),
                        TimeSpan.Parse(startTimeDayCompensation),
                        TimeSpan.Parse(endTimeDayCompensation));
                }

                MealCompensation dayEveningCompensation = null;
                if (string.IsNullOrWhiteSpace(startTimeDayEveningCompensation) || string.IsNullOrWhiteSpace(endTimeDayEveningCompensation))
                {
                    dayEveningCompensation = new MealCompensation(
                        0,
                        TimeSpan.Parse("00:00"),
                        TimeSpan.Parse("00:00"));
                }
                else
                {
                    decimal dayEveningComp;
                    decimal.TryParse(dayEveningCompensationValue, out dayEveningComp);

                    dayEveningCompensation = new MealCompensation(
                        dayEveningComp,
                        TimeSpan.Parse(startTimeDayEveningCompensation),
                        TimeSpan.Parse(endTimeDayEveningCompensation));
                }
                

                config = new Config(pathToSaveReports, dayCompensation, dayEveningCompensation);
            });

            return config;
        }
    }
}