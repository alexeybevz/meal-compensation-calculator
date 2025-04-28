using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MealCompensationCalculator.Domain.Models;
using MealCompensationCalculator.Domain.Queries;
using Moq;
using Xunit;

namespace MealCompensationCalculator.Test
{
    public class GetTimeSheetOfEmployeesQueryTest
    {
        [Fact]
        public void GetTimeSheetOfEmployeesQueryMockTest()
        {
            var mock = new GetTimeSheetOfEmployeesQueryMock().Execute().Object;
            var timeSheetOfEmployees = mock.Execute().Result;

            Assert.True(timeSheetOfEmployees.EmployeesTimeSheets.SelectMany(x => x.TimeSheetDays).Count() == 1);
        }

        class GetTimeSheetOfEmployeesQueryMock : Mock<IGetTimeSheetOfEmployeesQuery>
        {
            public GetTimeSheetOfEmployeesQueryMock Execute()
            {
                var employee = new EmployeeFromTimeSheet(777, "Фамилия И. О., Программист");

                var timeSheetDays = new List<TimeSheetDay>()
                {
                    new TimeSheetDay(1, "Я", "8"),
                };

                var employeeTimeSheet = new List<EmployeeTimeSheet>() { new EmployeeTimeSheet(employee, timeSheetDays.ToDictionary(x => x.Day)) };
                var timeSheetOfEmployees = new TimeSheetOfEmployees(employeeTimeSheet);

                Setup(x => x.Execute())
                    .Returns(Task.FromResult(timeSheetOfEmployees));

                return this;
            }
        }
    }
}