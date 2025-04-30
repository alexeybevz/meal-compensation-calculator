using System.Collections.Generic;
using System.Linq;
using MealCompensationCalculator.Domain.Models;
using MealCompensationCalculator.Domain.Services;

namespace MealCompensationCalculator.BusinessLogic.Services.CompensationCalculator
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
                var paysByDays = employeeTotalPayment
                    .Payments
                    .GroupBy(x => x.TransactionDateTime.Day)
                    .ToDictionary(x => x.Key, x => x.ToList());

                var compensationByDays = paysByDays.Keys.ToDictionary(x => x, x => new CompensationTimeSheetDay(0m, string.Empty, string.Empty));

                var timeSheetEmployees = _employeeMapper.GetEmployeeFromTimeSheets(timeSheetOfEmployees, employeeTotalPayment.Employee).ToList();
                if (!timeSheetEmployees.Any())
                {
                    results.Add(new CompensationResult(employeeTotalPayment, 0, compensationByDays));
                    continue;
                }

                var timeSheetDaysByDay = timeSheetEmployees
                    .SelectMany(x => x.TimeSheetDays)
                    .GroupBy(x => x.Key)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault().Value);

                var employeeTotalCompensation = paysByDays.Sum(paysByDay =>
                {
                    TimeSheetDay day;
                    if (!timeSheetDaysByDay.TryGetValue(paysByDay.Key, out day))
                        return 0m;

                    var compensation = _compensationTypeCalculators
                        .Where(x => x.CanApply(day.ScheduleOfWork, day.Shift))
                        .Sum(x => x.Execute(paysByDay.Value));

                    compensationByDays[paysByDay.Key] = new CompensationTimeSheetDay(compensation, day.ScheduleOfWork, day.Shift);

                    return compensation;
                });

                results.Add(new CompensationResult(employeeTotalPayment, employeeTotalCompensation, compensationByDays));
            }

            return results;
        }
    }
}