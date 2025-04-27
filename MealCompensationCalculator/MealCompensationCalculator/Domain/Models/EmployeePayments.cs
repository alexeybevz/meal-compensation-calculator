using System;
using System.Collections.Generic;

namespace MealCompensationCalculator.Domain.Models
{
    public class EmployeePayments
    {
        public Employee Employee { get; }
        public IEnumerable<Payment> Payments { get; }

        public EmployeePayments(Employee employee, IEnumerable<Payment> payments)
        {
            Employee = employee ?? throw new ArgumentException("employee is null");
            Payments = payments ?? throw new ArgumentException("payments is null");
        }
    }
}