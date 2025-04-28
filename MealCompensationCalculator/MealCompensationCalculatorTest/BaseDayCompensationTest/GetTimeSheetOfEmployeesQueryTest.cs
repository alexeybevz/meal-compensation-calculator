using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MealCompensationCalculator.Domain.Models;
using MealCompensationCalculator.Domain.Queries;
using Moq;
using Xunit;

namespace MealCompensationCalculatorTest.BaseDayCompensationTest
{
    public class GetTimeSheetOfEmployeesQueryTest
    {
        [Fact]
        public void GetTimeSheetOfEmployeesQueryMockTest()
        {
            var mock = new GetTimeSheetOfEmployeesQueryMock().Execute().Object;
            var timeSheetOfEmployees = mock.Execute().Result;

            Assert.True(timeSheetOfEmployees.EmployeesTimeSheets.SelectMany(x => x.TimeSheetDays).Count() == 31);
        }
    }

    internal class GetTimeSheetOfEmployeesQueryMock : Mock<IGetTimeSheetOfEmployeesQuery>
    {
        public GetTimeSheetOfEmployeesQueryMock Execute()
        {
            var employee = new EmployeeFromTimeSheet(777, "Фамилия И. О., Программист");

            var timeSheetDays = new List<TimeSheetDay>()
            {
                new TimeSheetDay(1, "В", ""),
                new TimeSheetDay(2, "Я", "8"),
                new TimeSheetDay(3, "Я", "8"),
                new TimeSheetDay(4, "Я", "8"),
                new TimeSheetDay(5, "Я", "8"),
                new TimeSheetDay(6, "Я", "8"),
                new TimeSheetDay(7, "В", ""),
                new TimeSheetDay(8, "В", ""),
                new TimeSheetDay(9, "ОТ", ""),
                new TimeSheetDay(10, "ОТ", ""),
                new TimeSheetDay(11, "ОТ", ""),
                new TimeSheetDay(12, "ОТ", ""),
                new TimeSheetDay(13, "ОТ", ""),
                new TimeSheetDay(14, "ОТ", ""),
                new TimeSheetDay(15, "ОТ", ""),
                new TimeSheetDay(16, "ОТ", ""),
                new TimeSheetDay(17, "ОТ", ""),
                new TimeSheetDay(18, "ОТ", ""),
                new TimeSheetDay(19, "ОТ", ""),
                new TimeSheetDay(20, "ОТ", ""),
                new TimeSheetDay(21, "ОТ", ""),
                new TimeSheetDay(22, "ОТ", ""),
                new TimeSheetDay(23, "Я", "8"),
                new TimeSheetDay(24, "Я", "8"),
                new TimeSheetDay(25, "Я", "8"),
                new TimeSheetDay(26, "Я", "8"),
                new TimeSheetDay(27, "Я", "8"),
                new TimeSheetDay(28, "В", ""),
                new TimeSheetDay(29, "В", ""),
                new TimeSheetDay(30, "Я", "8"),
                new TimeSheetDay(31, "Я", "8"),
            };

            var employeeTimeSheet = new List<EmployeeTimeSheet>() {new EmployeeTimeSheet(employee, timeSheetDays.ToDictionary(x => x.Day))};
            var timeSheetOfEmployees = new TimeSheetOfEmployees(employeeTimeSheet);

            Setup(x => x.Execute())
                .Returns(Task.FromResult(timeSheetOfEmployees));

            return this;
        }
    }
}