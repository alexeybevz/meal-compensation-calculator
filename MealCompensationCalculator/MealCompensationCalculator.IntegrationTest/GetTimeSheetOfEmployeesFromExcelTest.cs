using System.Linq;
using MealCompensationCalculator.BusinessLogic.Queries;
using Xunit;

namespace MealCompensationCalculator.IntegrationTest
{
    public class GetTimeSheetOfEmployeesFromExcelTest
    {
        [Fact]
        public async void Execute()
        {
            var service = new GetTimeSheetOfEmployeesFromExcel(PathToFilesStore.PathToTimeSheetOfEmployees);
            var result = await service.Execute();

            Assert.True(result.EmployeesTimeSheets.Count() == 1);
        }
    }
}