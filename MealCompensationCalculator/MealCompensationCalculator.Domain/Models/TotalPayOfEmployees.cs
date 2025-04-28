using System.Collections.Generic;

namespace MealCompensationCalculator.Domain.Models
{
    public class TotalPayOfEmployees
    {
        public IEnumerable<EmployeePayments> EmployeesTotalPayments { get; }

        public TotalPayOfEmployees(IEnumerable<EmployeePayments> employeesPayments)
        {
            EmployeesTotalPayments = employeesPayments;
        }
    }
}