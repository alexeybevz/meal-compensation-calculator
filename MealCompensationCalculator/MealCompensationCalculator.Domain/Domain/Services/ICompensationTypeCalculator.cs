using System.Collections.Generic;
using MealCompensationCalculator.Domain.Domain.Models;

namespace MealCompensationCalculator.Domain.Domain.Services
{
    public interface ICompensationTypeCalculator
    {
        decimal Execute(IEnumerable<Payment> payments);
        bool CanApply(string scheduleOfWork, string shift);
    }
}