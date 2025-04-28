using System.Collections.Generic;

namespace MealCompensationCalculator.Domain.Models
{
    public class TimeSheetOfEmployees
    {
        public IEnumerable<EmployeeTimeSheet> EmployeesTimeSheets { get; }

        public TimeSheetOfEmployees(IEnumerable<EmployeeTimeSheet> employeeTimeSheets)
        {
            EmployeesTimeSheets = employeeTimeSheets;
        }
    }
}