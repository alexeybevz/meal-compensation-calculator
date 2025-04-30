using System;
using System.Collections.Generic;

namespace MealCompensationCalculator.Domain.Models
{
    public class TotalPayOfEmployees
    {
        public DateTime StartPeriod { get; }
        public DateTime EndPeriod { get; }
        public IEnumerable<EmployeePayments> EmployeesTotalPayments { get; }

        public TotalPayOfEmployees(DateTime startPeriod, DateTime endPeriod, IEnumerable<EmployeePayments> employeesPayments)
        {
            StartPeriod = startPeriod;
            EndPeriod = endPeriod;
            EmployeesTotalPayments = employeesPayments;
        }
    }
}