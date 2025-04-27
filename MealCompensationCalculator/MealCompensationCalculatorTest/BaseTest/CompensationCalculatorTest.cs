using System.Linq;
using MealCompensationCalculator.Domain.Models;
using Xunit;

namespace MealCompensationCalculatorTest.BaseTest
{
    public class CompensationCalculatorTest
    {
        [Fact]
        public void CalcDayCompensationTest()
        {
            var getTotalPayOfEmployeesQueryMock = new GetTotalPayOfEmployeesQueryMock().Execute().Object;
            var totalPayOfEmployees = getTotalPayOfEmployeesQueryMock.Execute().Result;

            var getTimeSheetOfEmployeesQueryMock = new GetTimeSheetOfEmployeesQueryMock().Execute().Object;
            var timeSheetOfEmployees = getTimeSheetOfEmployeesQueryMock.Execute().Result;

            var dayCompensation = new MealCompensation()
            {
                Compensation = 70,
                StartCompensationHour = 11,
                StartCompensationMinute = 0,
                EndCompensationHour = 14,
                EndCompensationMinute = 0
            };

            var dayEveningCompensation = new MealCompensation()
            {
                Compensation = 110,
                StartCompensationHour = 16,
                StartCompensationMinute = 30,
                EndCompensationHour = 17,
                EndCompensationMinute = 30
            };

            var compensationCalculator = new CompensationCalculator(dayCompensation, dayEveningCompensation);
            var result = compensationCalculator.Execute(totalPayOfEmployees, timeSheetOfEmployees);

            Assert.True(result.Sum(x => x.Compensation) == 833);
        }
    }
}