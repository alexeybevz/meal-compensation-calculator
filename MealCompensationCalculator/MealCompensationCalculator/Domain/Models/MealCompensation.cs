using System;
using System.Collections.Generic;
using System.Linq;

namespace MealCompensationCalculator.Domain.Models
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

    internal class EmployeeMapper
    {
        public List<EmployeeTimeSheet> GetEmployeeFromTimeSheets(TimeSheetOfEmployees timeSheetOfEmployees, Employee employee)
        {
            return timeSheetOfEmployees.EmployeesTimeSheets
                .Where(x => x.Employee.EmployeeNumber == employee.EmployeeNumber).ToList();
        }
    }

    public class CompensationCalculator
    {
        private readonly IEnumerable<ICompensationTypeCalculator> _compensationTypeCalculators;
        private readonly EmployeeMapper _employeeMapper;

        public CompensationCalculator(MealCompensation dayCompensation, MealCompensation dayEveningCompensation)
        {
            _compensationTypeCalculators = new List<ICompensationTypeCalculator>()
            {
                new DayCompensationCalculator(dayCompensation, dayEveningCompensation),
                new DayEveningCompensationCalculator(dayCompensation, dayEveningCompensation),
            };
            _employeeMapper = new EmployeeMapper();
        }

        public List<CompensationResult> Execute(TotalPayOfEmployees totalPayOfEmployees, TimeSheetOfEmployees timeSheetOfEmployees)
        {
            var result = new List<CompensationResult>();

            foreach (var employeeTotalPayment in totalPayOfEmployees.EmployeesTotalPayments)
            {
                var employeeFromTimeSheet = _employeeMapper.GetEmployeeFromTimeSheets(timeSheetOfEmployees, employeeTotalPayment.Employee);
                if (employeeFromTimeSheet == null || !employeeFromTimeSheet.Any())
                    continue;

                var empPays = employeeTotalPayment.Payments.GroupBy(x => x.TransactionDateTime.Day).Select(x => new
                {
                    Day = x.Key,
                    Pays = x.ToList()
                }).ToList();

                decimal employeeTotalCompensation = 0;

                foreach (var empPay in empPays)
                {
                    var day = employeeFromTimeSheet.SelectMany(x => x.TimeSheetDays)
                        .FirstOrDefault(x => x.Day == empPay.Day);

                    if (day == null)
                        continue;

                    var compensation = _compensationTypeCalculators
                        .Where(x => x.CanApply(day.ScheduleOfWork, day.Shift))
                        .Sum(x => x.Execute(empPay.Pays));

                    employeeTotalCompensation += compensation;
                }

                result.Add(new CompensationResult(employeeTotalPayment, employeeTotalCompensation));
            }

            return result;
        }
    }

    public class CompensationResult
    {
        public EmployeePayments EmployeePayments { get; }
        public decimal Compensation { get; }

        public CompensationResult(EmployeePayments employeePayments, decimal compensation)
        {
            EmployeePayments = employeePayments;
            Compensation = compensation;
        }
    }

    internal interface ICompensationTypeCalculator
    {
        decimal Execute(IEnumerable<Payment> payments);
        bool CanApply(string scheduleOfWork, string shift);
    }

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
            var shiftParseResult = decimal.TryParse(shift, out var shiftDecimal);

            return (daySOWs.Contains(scheduleOfWork) && !string.IsNullOrEmpty(shift)
                    || shiftParseResult && scheduleOfWork == "Я" && shiftDecimal <= 8);
        }
    }

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
            var shiftParseResult = decimal.TryParse(shift, out var shiftDecimal);

            return (dayEveningSOWs.Any(x => scheduleOfWork.Contains(x)) && !string.IsNullOrEmpty(shift)
                    || shiftParseResult && scheduleOfWork == "Я" && shiftDecimal > 8);
        }
    }
}