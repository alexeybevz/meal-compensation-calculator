using System;
using System.Collections.Generic;

namespace MealCompensationCalculator.Domain.Models
{
    public class TimeSheetOfEmployees
    {
        public DateTime StartPeriod { get; }
        public DateTime EndPeriod { get; }
        public IEnumerable<EmployeeTimeSheet> EmployeesTimeSheets { get; }

        public TimeSheetOfEmployees(DateTime startPeriod, DateTime endPeriod, IEnumerable<EmployeeTimeSheet> employeeTimeSheets)
        {
            StartPeriod = startPeriod;
            EndPeriod = endPeriod;
            EmployeesTimeSheets = employeeTimeSheets;
        }
    }
}