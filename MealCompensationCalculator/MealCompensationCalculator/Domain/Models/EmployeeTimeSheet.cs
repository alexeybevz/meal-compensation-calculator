using System;
using System.Collections.Generic;

namespace MealCompensationCalculator.Domain.Models
{
    public class EmployeeTimeSheet
    {
        public EmployeeFromTimeSheet Employee { get; }
        public IReadOnlyDictionary<int, TimeSheetDay> TimeSheetDays { get; }

        public EmployeeTimeSheet(EmployeeFromTimeSheet employee, IReadOnlyDictionary<int, TimeSheetDay> timeSheetDays)
        {
            Employee = employee ?? throw new ArgumentException("employee is null");
            TimeSheetDays = timeSheetDays ?? throw new ArgumentException("payments is null");
        }
    }
}