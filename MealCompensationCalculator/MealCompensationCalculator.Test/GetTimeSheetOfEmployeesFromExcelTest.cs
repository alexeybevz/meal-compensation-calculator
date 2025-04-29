using System.Linq;
using MealCompensationCalculator.BusinessLogic.Queries;
using Xunit;

namespace MealCompensationCalculator.Test
{
    public class GetTimeSheetOfEmployeesFromExcelTest
    {
        [Fact]
        public async void Execute()
        {
            var path = @"C:\Users\abevz\Desktop\табель.xlsx";

            var service = new GetTimeSheetOfEmployeesFromExcel(path);
            var result = await service.Execute();

            Assert.True(result.EmployeesTimeSheets.Count() == 1159);
            Assert.True(result.EmployeesTimeSheets.Count(x => x.Employee.EmployeeNumber == 1049) == 2);
        }
    }
}