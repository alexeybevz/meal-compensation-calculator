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
        private readonly TypeCompensationCalculatorFactory _compensationCalculatorFactory;
        private readonly EmployeeMapper _employeeMapper;

        public CompensationCalculator(MealCompensation dayCompensation, MealCompensation dayEveningCompensation)
        {
            _compensationCalculatorFactory = new TypeCompensationCalculatorFactory(dayCompensation, dayEveningCompensation);
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

                    var typeCompensation = GetTypeCompensation(day.ScheduleOfWork, day.Shift);

                    var compensationCalculator = _compensationCalculatorFactory.GetCalculator(typeCompensation);
                    var compensation = compensationCalculator.Execute(empPay.Pays);

                    employeeTotalCompensation += compensation;
                }

                result.Add(new CompensationResult(employeeTotalPayment, employeeTotalCompensation));
            }

            return result;
        }

        private TypeCompensation GetTypeCompensation(string scheduleOfWork, string shift)
        {
            decimal shiftDecimal;
            var shiftParseResult = Decimal.TryParse(shift, out shiftDecimal);

            if (

                ((
                    (scheduleOfWork == "ПК")
                    || (scheduleOfWork == "Я/ПК")
                    || (scheduleOfWork == "Я/ПК/Г")
                    || (scheduleOfWork == "Я/Г")
                    || (scheduleOfWork == "Я/С")
                    || (scheduleOfWork == "Я/ДС")
                ) && (shift != ""))
                || (shiftParseResult && (scheduleOfWork == "Я") && (shiftDecimal <= 8))
            )
                return TypeCompensation.Day;

            return TypeCompensation.NotDefined;
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

    internal interface ITypeCompensationCalculator
    {
        decimal Execute(IEnumerable<Payment> payments);
    }

    internal class DayCompensationCalculator : ITypeCompensationCalculator
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
    }

    internal class DayEveningCompensationCalculator : ITypeCompensationCalculator
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
    }

    internal class NotDefinedCompensationCalculator : ITypeCompensationCalculator
    {
        public decimal Execute(IEnumerable<Payment> payments)
        {
            return 0;
        }
    }

    internal class TypeCompensationCalculatorFactory
    {
        private readonly ITypeCompensationCalculator _dayCompensationCalculator;
        private readonly ITypeCompensationCalculator _dayEveningCompensationCalculator;
        private readonly ITypeCompensationCalculator _notDefinedCalculator;

        public TypeCompensationCalculatorFactory(MealCompensation dayCompensation, MealCompensation dayEveningCompensation)
        {
            _dayCompensationCalculator = new DayCompensationCalculator(dayCompensation, dayEveningCompensation);
            _dayEveningCompensationCalculator = new DayEveningCompensationCalculator(dayCompensation, dayEveningCompensation);
            _notDefinedCalculator = new NotDefinedCompensationCalculator();
        }

        public ITypeCompensationCalculator GetCalculator(TypeCompensation typeCompensation)
        {
            switch (typeCompensation)
            {
                case TypeCompensation.Day:
                    return _dayCompensationCalculator;
                case TypeCompensation.DayEvening:
                    return _dayEveningCompensationCalculator;
                case TypeCompensation.NotDefined:
                    return _notDefinedCalculator;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    internal enum TypeCompensation
    {
        NotDefined,
        Day,
        DayEvening
    }
}