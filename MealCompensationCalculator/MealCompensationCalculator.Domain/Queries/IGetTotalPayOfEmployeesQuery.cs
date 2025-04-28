using System.Threading.Tasks;
using MealCompensationCalculator.Domain.Models;

namespace MealCompensationCalculator.Domain.Queries
{
    public interface IGetTotalPayOfEmployeesQuery
    {
        Task<TotalPayOfEmployees> Execute();
    }
}