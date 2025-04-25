using System.Threading.Tasks;
using MealCompensationCalculator.Domain.Models;

namespace MealCompensationCalculator.Domain.Queries
{
    public interface IGetTimeSheetOfEmployeesQuery
    {
        Task<TimeSheetOfEmployees> Execute();
    }
}