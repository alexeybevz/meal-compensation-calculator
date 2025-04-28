using System.Threading.Tasks;
using MealCompensationCalculator.Domain.Domain.Models;

namespace MealCompensationCalculator.Domain.Domain.Queries
{
    public interface IGetTimeSheetOfEmployeesQuery
    {
        Task<TimeSheetOfEmployees> Execute();
    }
}