using System.Collections.Generic;

namespace MealCompensationCalculator.Domain.Domain.Models
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