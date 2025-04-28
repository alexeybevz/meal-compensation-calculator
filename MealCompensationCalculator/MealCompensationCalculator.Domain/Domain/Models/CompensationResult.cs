using System.Collections.Generic;

namespace MealCompensationCalculator.Domain.Domain.Models
{
    public class CompensationResult
    {
        public EmployeePayments EmployeePayments { get; }
        public decimal TotalCompensation { get; }
        public IReadOnlyDictionary<int, decimal> CompensationByDays { get; }

        public CompensationResult(EmployeePayments employeePayments, decimal totalCompensation, IReadOnlyDictionary<int, decimal> compensationByDays)
        {
            EmployeePayments = employeePayments;
            TotalCompensation = totalCompensation;
            CompensationByDays = compensationByDays;
        }
    }
}