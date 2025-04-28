using System.Linq;
using MealCompensationCalculator.BusinessLogic.Queries;
using Xunit;

namespace MealCompensationCalculator.Test
{
    public class GetTotalPayOfEmployeesFromExcelTest
    {
        [Fact]
        public async void Execute()
        {
            var path = @"C:\Users\abevz\Desktop\отчет по сотрудникам октябрь.xlsx";

            var service = new GetTotalPayOfEmployeesFromExcel(path);
            var result = await service.Execute();

            var expected = 1019843m;
            var actual = result.EmployeesTotalPayments.SelectMany(x => x.Payments).Sum(x => x.Cost);
            
            Assert.True(expected == actual);
            Assert.True(result.EmployeesTotalPayments.Count() == 794);
        }
    }
}