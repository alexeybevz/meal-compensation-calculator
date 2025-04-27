using System.Collections.Generic;
using System.Linq;
using MealCompensationCalculator.Domain.Models;

namespace MealCompensationCalculator.Services
{
    internal class EmployeeMapper
    {
        public IEnumerable<EmployeeTimeSheet> GetEmployeeFromTimeSheets(TimeSheetOfEmployees timeSheetOfEmployees, Employee employee)
        {
            return timeSheetOfEmployees.EmployeesTimeSheets
                .Where(x => x.Employee.EmployeeNumber == employee.EmployeeNumber).ToList();
        }
    }
}