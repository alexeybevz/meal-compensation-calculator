using System;
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
        public async void GetTimeSheetOfEmployeesQueryMockTest()
        {
            var mock = new GetTimeSheetOfEmployeesQueryMock().Execute().Object;
            var timeSheetOfEmployees = await mock.Execute();

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
                var timeSheetOfEmployees = new TimeSheetOfEmployees(DateTime.Parse("01.10.2017"), DateTime.Parse("31.10.2017"), employeeTimeSheet);

                Setup(x => x.Execute())
                    .Returns(Task.FromResult(timeSheetOfEmployees));

                return this;
            }
        }
    }
}