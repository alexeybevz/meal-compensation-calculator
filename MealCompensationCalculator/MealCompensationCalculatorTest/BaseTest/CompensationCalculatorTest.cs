using System;
using System.Linq;
using MealCompensationCalculator.Domain.Models;
using MealCompensationCalculator.Services;
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

            var dayCompensation = new MealCompensation(70, new TimeSpan(11, 0, 0), new TimeSpan(14, 0, 0));
            var dayEveningCompensation = new MealCompensation(110, new TimeSpan(16, 30, 0), new TimeSpan(17, 30, 0));

            var compensationCalculator = new CompensationCalculator(dayCompensation, dayEveningCompensation);
            var result = compensationCalculator.Execute(totalPayOfEmployees, timeSheetOfEmployees);

            Assert.True(result.Sum(x => x.Compensation) == 833);
        }
    }
}