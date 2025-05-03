namespace MealCompensationCalculator.Domain.Models
{
    public class Config
    {
        public string PathToSaveReports { get; }
        public MealCompensation DayCompensation { get; }
        public MealCompensation DayEveningCompensation { get; }

        public Config(string pathToSaveReports, MealCompensation dayCompensation, MealCompensation dayEveningCompensation)
        {
            PathToSaveReports = pathToSaveReports;
            DayCompensation = dayCompensation;
            DayEveningCompensation = dayEveningCompensation;
        }
    }
}