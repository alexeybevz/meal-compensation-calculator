namespace MealCompensationCalculator.Domain.Models
{
    public class CompensationTimeSheetDay
    {
        public decimal Compensation { get; }
        public string ScheduleOfWork { get; }
        public string Shift { get; }

        public CompensationTimeSheetDay(decimal compensation, string scheduleOfWork, string shift)
        {
            Compensation = compensation;
            ScheduleOfWork = scheduleOfWork;
            Shift = shift;
        }
    }
}