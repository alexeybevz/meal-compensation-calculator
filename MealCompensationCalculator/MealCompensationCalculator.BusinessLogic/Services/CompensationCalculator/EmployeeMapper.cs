using System.Collections.Generic;
using System.Linq;
using MealCompensationCalculator.Domain.Domain.Models;

namespace MealCompensationCalculator.BusinessLogic.Services.CompensationCalculator
{
    internal class EmployeeMapper
    {
        public IEnumerable<EmployeeTimeSheet> GetEmployeeFromTimeSheets(TimeSheetOfEmployees timeSheetOfEmployees, Employee employee)
        {
            if (timeSheetOfEmployees == null)
                return new List<EmployeeTimeSheet>();

            return timeSheetOfEmployees.EmployeesTimeSheets?
                .Where(x => x.Employee.EmployeeNumber == employee.EmployeeNumber).ToList() ?? new List<EmployeeTimeSheet>();
        }
    }
}