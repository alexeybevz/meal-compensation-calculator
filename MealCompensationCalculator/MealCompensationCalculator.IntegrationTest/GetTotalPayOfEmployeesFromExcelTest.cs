using System.Linq;
using MealCompensationCalculator.BusinessLogic.Queries;
using Xunit;

namespace MealCompensationCalculator.IntegrationTest
{
    public class GetTotalPayOfEmployeesFromExcelTest
    {
        [Fact]
        public async void Execute()
        {
            var service = new GetTotalPayOfEmployeesFromExcel(PathToFilesStore.PathToTotalPayOfEmployees);
            var result = await service.Execute();

            var expected = 1263m;
            var actual = result.EmployeesTotalPayments.SelectMany(x => x.Payments).Sum(x => x.Cost);
            
            Assert.True(expected == actual);
            Assert.True(result.EmployeesTotalPayments.Count() == 1);
        }
    }
}