using System.Collections.Generic;
using MealCompensationCalculator.Domain.Models;

namespace MealCompensationCalculator.Domain.Services
{
    public interface ICompensationTypeCalculator
    {
        decimal Execute(IEnumerable<Payment> payments);
        bool CanApply(string scheduleOfWork, string shift);
    }
}