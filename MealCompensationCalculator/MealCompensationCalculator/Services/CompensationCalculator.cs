using System.Collections.Generic;
using System.Linq;
using MealCompensationCalculator.Domain.Models;
using MealCompensationCalculator.Domain.Services;

namespace MealCompensationCalculator.Services
{
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
            var results = new List<CompensationResult>();

            foreach (var employeeTotalPayment in totalPayOfEmployees.EmployeesTotalPayments)
            {
                var timeSheetEmployees = _employeeMapper.GetEmployeeFromTimeSheets(timeSheetOfEmployees, employeeTotalPayment.Employee).ToList();
                if (!timeSheetEmployees.Any())
                {
                    results.Add(new CompensationResult(employeeTotalPayment, 0));
                    continue;
                }

                var timeSheetDaysByDay = timeSheetEmployees
                    .SelectMany(x => x.TimeSheetDays)
                    .GroupBy(x => x.Key)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault().Value);

                var paysByDays = employeeTotalPayment
                    .Payments
                    .GroupBy(x => x.TransactionDateTime.Day)
                    .ToDictionary(x => x.Key, x => x.ToList());

                var employeeTotalCompensation = paysByDays.Sum(paysByDay =>
                {
                    TimeSheetDay day;
                    if (!timeSheetDaysByDay.TryGetValue(paysByDay.Key, out day))
                        return 0m;

                    return _compensationTypeCalculators
                        .Where(x => x.CanApply(day.ScheduleOfWork, day.Shift))
                        .Sum(x => x.Execute(paysByDay.Value));
                });

                results.Add(new CompensationResult(employeeTotalPayment, employeeTotalCompensation));
            }

            return results;
        }
    }
}