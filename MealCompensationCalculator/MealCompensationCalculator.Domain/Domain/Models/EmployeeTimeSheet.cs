using System;
using System.Collections.Generic;

namespace MealCompensationCalculator.Domain.Domain.Models
{
    public class EmployeeTimeSheet
    {
        public EmployeeFromTimeSheet Employee { get; }
        public IReadOnlyDictionary<int, TimeSheetDay> TimeSheetDays { get; }

        public EmployeeTimeSheet(EmployeeFromTimeSheet employee, IReadOnlyDictionary<int, TimeSheetDay> timeSheetDays)
        {
            if (employee == null)
                throw new ArgumentException("employee is null");

            if (timeSheetDays == null)
                throw new ArgumentException("payments is null");

            Employee = employee;
            TimeSheetDays = timeSheetDays;
        }
    }
}