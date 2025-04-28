using System;

namespace MealCompensationCalculator.Domain.Domain.Models
{
    public class MealCompensation
    {
        public TimeSpan StartTimeCompensation { get; }
        public TimeSpan EndTimeCompensation { get; }

        public decimal Compensation { get; }

        public MealCompensation(decimal compensation, TimeSpan startTimeCompensation, TimeSpan endTimeCompensation)
        {
            Compensation = compensation;
            StartTimeCompensation = startTimeCompensation;
            EndTimeCompensation = endTimeCompensation;
        }

        public bool IsDateFallsToCompensationPeriod(DateTime dateTimeTransaction)
        {
            return dateTimeTransaction.TimeOfDay >= StartTimeCompensation &&
                   dateTimeTransaction.TimeOfDay <= EndTimeCompensation;
        }
    }
}