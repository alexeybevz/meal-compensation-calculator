using System;
using System.Collections.Generic;

namespace MealCompensationCalculator.Domain.Models
{
    public class EmployeeTimeSheet
    {
        public EmployeeFromTimeSheet Employee { get; }
        public IEnumerable<TimeSheetDay> TimeSheetDays { get; }

        public EmployeeTimeSheet(EmployeeFromTimeSheet employee, IEnumerable<TimeSheetDay> timeSheetDays)
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