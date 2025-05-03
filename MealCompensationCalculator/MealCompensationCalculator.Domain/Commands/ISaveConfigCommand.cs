using System.Threading.Tasks;
using MealCompensationCalculator.Domain.Models;

namespace MealCompensationCalculator.Domain.Commands
{
    public interface ISaveConfigCommand
    {
        Task Execute(Config config);
    }
}