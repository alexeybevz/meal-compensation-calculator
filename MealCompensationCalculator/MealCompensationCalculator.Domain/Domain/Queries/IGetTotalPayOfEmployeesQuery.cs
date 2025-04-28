using System.Threading.Tasks;
using MealCompensationCalculator.Domain.Domain.Models;

namespace MealCompensationCalculator.Domain.Domain.Queries
{
    public interface IGetTotalPayOfEmployeesQuery
    {
        Task<TotalPayOfEmployees> Execute();
    }
}