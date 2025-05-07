using System.Collections.Generic;

namespace MealCompensationCalculator.Domain.Models
{
    public class CompensationResult
    {
        public EmployeePayments EmployeePayments { get; }
        public decimal TotalCompensation { get; }
        public IReadOnlyDictionary<int, CompensationTimeSheetDay> CompensationByDays { get; }

        public CompensationResult(EmployeePayments employeePayments, decimal totalCompensation, IReadOnlyDictionary<int, CompensationTimeSheetDay> compensationByDays)
        {
            EmployeePayments = employeePayments;
            TotalCompensation = totalCompensation;
            CompensationByDays = compensationByDays;
        }
    }
}