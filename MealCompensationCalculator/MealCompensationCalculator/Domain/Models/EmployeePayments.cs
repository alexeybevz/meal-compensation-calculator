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
            if (employee == null)
                throw new ArgumentException("employee is null");

            if (payments == null)
                throw new ArgumentException("payments is null");

            Employee = employee;
            Payments = payments;
        }
    }
}