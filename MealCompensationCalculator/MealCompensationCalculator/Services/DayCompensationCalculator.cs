using System.Collections.Generic;
using System.Linq;
using MealCompensationCalculator.Domain.Models;
using MealCompensationCalculator.Domain.Services;

namespace MealCompensationCalculator.Services
{
    internal class DayCompensationCalculator : ICompensationTypeCalculator
    {
        private readonly MealCompensation _dayCompensation;
        private readonly MealCompensation _dayEveningCompensation;

        public DayCompensationCalculator(MealCompensation dayCompensation, MealCompensation dayEveningCompensation)
        {
            _dayCompensation = dayCompensation;
            _dayEveningCompensation = dayEveningCompensation;
        }

        public decimal Execute(IEnumerable<Payment> payments)
        {
            var firstPayment = payments?.FirstOrDefault(x =>
                _dayCompensation.IsDateFallsToCompensationPeriod(x.TransactionDateTime) ||
                _dayEveningCompensation.IsDateFallsToCompensationPeriod(x.TransactionDateTime));
            
            if (firstPayment == null)
                return 0;

            return firstPayment.Cost <= _dayCompensation.Compensation
                ? firstPayment.Cost
                : _dayCompensation.Compensation;
        }

        public bool CanApply(string scheduleOfWork, string shift)
        {
            if (string.IsNullOrEmpty(scheduleOfWork))
                return false;

            var daySOWs = new[] { "ПК", "Я/ПК", "Я/ПК/Г", "Я/Г", "Я/С", "Я/ДС" };

            decimal shiftDecimal;
            var shiftParseResult = decimal.TryParse(shift, out shiftDecimal);

            return (daySOWs.Contains(scheduleOfWork) && !string.IsNullOrEmpty(shift)
                    || shiftParseResult && scheduleOfWork == "Я" && shiftDecimal <= 8);
        }
    }
}