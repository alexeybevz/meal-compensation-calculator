using System;
using System.Collections.Generic;
using System.Linq;

namespace MealCompensationCalculator.Domain.Models
{
    public class MealCompensation
    {
        public int StartCompensationHour { get; set; }
        public int StartCompensationMinute { get; set; }
        public int EndCompensationHour { get; set; }
        public int EndCompensationMinute { get; set; }

        public decimal Compensation { get; set; }
    }

    internal class EmployeeMapper
    {
        private readonly TimeSheetOfEmployees _timeSheetOfEmployees;

        public EmployeeMapper(TimeSheetOfEmployees timeSheetOfEmployees)
        {
            _timeSheetOfEmployees = timeSheetOfEmployees;
        }

        public List<EmployeeTimeSheet> GetEmployeeFromTimeSheets(Employee employee)
        {
            return _timeSheetOfEmployees.EmployeesTimeSheets
                .Where(x => x.Employee.EmployeeNumber == employee.EmployeeNumber).ToList();
        }
    }

    public class CompensationCalculator
    {
        private readonly TypeCompensationCalculatorFactory _compensationCalculatorFactory;

        public CompensationCalculator(MealCompensation dayCompensation, MealCompensation dayEveningCompensation)
        {
            _compensationCalculatorFactory = new TypeCompensationCalculatorFactory(dayCompensation, dayEveningCompensation);
        }

        public List<CompensationResult> Execute(TotalPayOfEmployees totalPayOfEmployees, TimeSheetOfEmployees timeSheetOfEmployees)
        {
            var result = new List<CompensationResult>();

            var employeeMapper = new EmployeeMapper(timeSheetOfEmployees);

            foreach (var employeeTotalPayment in totalPayOfEmployees.EmployeesTotalPayments)
            {
                var employeeFromTimeSheet = employeeMapper.GetEmployeeFromTimeSheets(employeeTotalPayment.Employee);
                if (employeeFromTimeSheet == null || !employeeFromTimeSheet.Any())
                    continue;

                var list = employeeTotalPayment.Payments.GroupBy(x => x.TransactionDateTime.Day).Select(x => new
                {
                    Day = x.Key,
                    Pays = x.ToList()
                }).ToList();

                decimal employeeTotalCompensation = 0;

                foreach (var grp in list)
                {
                    var obj = employeeFromTimeSheet.SelectMany(x => x.TimeSheetDays)
                        .FirstOrDefault(x => x.Day == grp.Day);

                    if (obj == null)
                        continue;

                    var typeCompensation = GetTypeCompensation(obj.ScheduleOfWork, obj.Shift);

                    var compensationCalculator = _compensationCalculatorFactory.GetCalculator(typeCompensation);
                    var compensation = compensationCalculator.Execute(grp.Pays);

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

    internal interface ICompensationCalculator
    {
        decimal Execute(IEnumerable<Payment> payments);
    }

    internal class DayCompensationCalculator : ICompensationCalculator
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
            var firstPayment = payments?.FirstOrDefault(x => IsValid(x.TransactionDateTime));
            if (firstPayment == null)
                return 0;

            return firstPayment.Cost <= _dayCompensation.Compensation
                ? firstPayment.Cost
                : _dayCompensation.Compensation;
        }

        private bool IsValid(DateTime dateTimeTransaction)
        {
            var date = dateTimeTransaction.Date;

            var d1 = new DateTime(date.Year, date.Month, date.Day, _dayCompensation.StartCompensationHour, _dayCompensation.StartCompensationMinute, 0);
            var d2 = new DateTime(date.Year, date.Month, date.Day, _dayCompensation.EndCompensationHour, _dayCompensation.StartCompensationMinute, 0);

            var d3 = new DateTime(date.Year, date.Month, date.Day, _dayEveningCompensation.StartCompensationHour, _dayEveningCompensation.StartCompensationMinute, 0);
            var d4 = new DateTime(date.Year, date.Month, date.Day, _dayEveningCompensation.EndCompensationHour, _dayEveningCompensation.StartCompensationMinute, 0);

            return d1 <= dateTimeTransaction && dateTimeTransaction <= d2 ||
                   d3 <= dateTimeTransaction && dateTimeTransaction <= d4;
        }
    }

    internal class DayEveningCompensationCalculator : ICompensationCalculator
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
            throw new System.NotImplementedException();
        }
    }

    internal class NotDefinedCompensationCalculator : ICompensationCalculator
    {
        private readonly MealCompensation _dayCompensation;
        private readonly MealCompensation _dayEveningCompensation;

        public NotDefinedCompensationCalculator(MealCompensation dayCompensation, MealCompensation dayEveningCompensation)
        {
            _dayCompensation = dayCompensation;
            _dayEveningCompensation = dayEveningCompensation;
        }

        public decimal Execute(IEnumerable<Payment> payments)
        {
            return 0;
        }
    }

    internal class TypeCompensationCalculatorFactory
    {
        private readonly ICompensationCalculator _dayCompensationCalculator;
        private readonly ICompensationCalculator _dayEveningCompensationCalculator;
        private readonly ICompensationCalculator _notDefinedCalculator;

        public TypeCompensationCalculatorFactory(MealCompensation dayCompensation, MealCompensation dayEveningCompensation)
        {
            _dayCompensationCalculator = new DayCompensationCalculator(dayCompensation, dayEveningCompensation);
            _dayEveningCompensationCalculator = new DayEveningCompensationCalculator(dayCompensation, dayEveningCompensation);
            _notDefinedCalculator = new NotDefinedCompensationCalculator(dayCompensation, dayEveningCompensation);
        }

        public ICompensationCalculator GetCalculator(TypeCompensation typeCompensation)
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