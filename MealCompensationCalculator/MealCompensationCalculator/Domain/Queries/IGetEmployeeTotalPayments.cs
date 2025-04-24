using System.Collections.Generic;
using System.Threading.Tasks;
using MealCompensationCalculator.Domain.Models;

namespace MealCompensationCalculator.Domain.Queries
{
    public interface IGetEmployeeTotalPaymentsQuery
    {
        Task<IEnumerable<EmployeeTotalPayment>> Execute();
    }
}