namespace MealCompensationCalculator.Domain.Domain.Models
{
    public class TimeSheetDay
    {
        /// <summary>
        /// День месяца
        /// </summary>
        public int Day { get; }
        /// <summary>
        /// График
        /// </summary>
        public string ScheduleOfWork { get; }
        /// <summary>
        /// Смена
        /// </summary>
        public string Shift { get; }

        /// <summary>
        /// День из табеля учета рабочего времени
        /// </summary>
        /// <param name="day">День месяца</param>
        /// <param name="scheduleOfWork">График</param>
        /// <param name="shift">Смена</param>
        public TimeSheetDay(int day, string scheduleOfWork, string shift)
        {
            Day = day;
            ScheduleOfWork = scheduleOfWork;
            Shift = shift;
        }
    }
}