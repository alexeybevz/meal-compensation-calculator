using System.Collections.Generic;
using System.Linq;
using MealCompensationCalculator.Domain.Domain.Models;
using MealCompensationCalculator.Domain.Domain.Services;

namespace MealCompensationCalculator.Domain.Services
{
    internal class DayEveningCompensationCalculator : ICompensationTypeCalculator
    {
        private readonly MealCompensation _dayCompensation;
        private readonly MealCompensation _dayEveningCompensation;

        public DayEveningCompensationCalculator(MealCompensation dayCompensation, MealCompensation dayEveningCompensation)
        {
            _dayCompensation = dayCompensation;
            _dayEveningCompensation = dayEveningCompensation;
        }

        public decimal Execute(IEnumerable<Payment> payments)
        {
            var totalCost = payments
                .Where(x =>
                    _dayCompensation.IsDateFallsToCompensationPeriod(x.TransactionDateTime) ||
                    _dayEveningCompensation.IsDateFallsToCompensationPeriod(x.TransactionDateTime))
                .Sum(x => x.Cost);

            return totalCost <= _dayEveningCompensation.Compensation
                ? totalCost
                : _dayEveningCompensation.Compensation;
        }

        public bool CanApply(string scheduleOfWork, string shift)
        {
            if (string.IsNullOrEmpty(scheduleOfWork))
                return false;

            var dayEveningSOWs = new[] { "Я/ВЧ", "Я/Н", "РВ", "РВ/ВЧ", "НП", "РП", "Я/ПК/ВЧ", "Я/С/ВЧ" };

            decimal shiftDecimal;
            var shiftParseResult = decimal.TryParse(shift, out shiftDecimal);

            return (dayEveningSOWs.Any(x => scheduleOfWork.Contains(x)) && !string.IsNullOrEmpty(shift)
                    || shiftParseResult && scheduleOfWork == "Я" && shiftDecimal > 8);
        }
    }
}