using System;
using System.Collections.Generic;

namespace MealCompensationCalculator.Domain.Models
{
    public class EmployeeTotalPayment
    {
        private readonly List<Payment> _payments;

        public Employee Employee { get; }
        public IEnumerable<Payment> Payments => _payments;

        public EmployeeTotalPayment(Employee employee)
        {
            if (employee == null)
                throw new ArgumentException("employee is null");

            Employee = employee;
            _payments = new List<Payment>();
        }

        public void AddPayment(Payment payment)
        {
            if (payment == null)
                throw new ArgumentException("payment is null");

            _payments.Add(payment);
        }
    }
}