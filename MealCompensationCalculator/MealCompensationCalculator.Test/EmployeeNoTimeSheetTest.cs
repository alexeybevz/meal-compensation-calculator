using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MealCompensationCalculator.Domain.Models;
using MealCompensationCalculator.Domain.Queries;
using Moq;
using Xunit;
using MealCompensationCalculator.BusinessLogic.Services.CompensationCalculator;

namespace MealCompensationCalculator.Test
{
    public class EmployeeNoTimeSheetTest
    {
        [Fact]
        public void TimeSheetOfEmployeesIsNullTest()
        {
            var getTotalPayOfEmployeesQueryMock = new GetTotalPayOfEmployeesQueryMock().Execute().Object;
            var totalPayOfEmployees = getTotalPayOfEmployeesQueryMock.Execute().Result;

            var getTimeSheetOfEmployeesQueryMock = new GetNullTimeSheetOfEmployeesQueryMock().Execute().Object;
            var timeSheetOfEmployees = getTimeSheetOfEmployeesQueryMock.Execute().Result;

            var dayCompensation = new MealCompensation(70, new TimeSpan(11, 0, 0), new TimeSpan(14, 0, 0));
            var dayEveningCompensation = new MealCompensation(110, new TimeSpan(16, 30, 0), new TimeSpan(17, 30, 0));

            var compensationCalculator = new CompensationCalculator(dayCompensation, dayEveningCompensation);
            var result = compensationCalculator.Execute(totalPayOfEmployees, timeSheetOfEmployees);

            Assert.True(result.Sum(x => x.TotalCompensation) == 0);
        }

        [Fact]
        public void TimeSheetWithOtherEmployeeTest()
        {
            var getTotalPayOfEmployeesQueryMock = new GetTotalPayOfEmployeesQueryMock().Execute().Object;
            var totalPayOfEmployees = getTotalPayOfEmployeesQueryMock.Execute().Result;

            var getTimeSheetOfEmployeesQueryMock = new GetTimeSheetWithOtherEmployeesQueryMock().Execute().Object;
            var timeSheetOfEmployees = getTimeSheetOfEmployeesQueryMock.Execute().Result;

            var dayCompensation = new MealCompensation(70, new TimeSpan(11, 0, 0), new TimeSpan(14, 0, 0));
            var dayEveningCompensation = new MealCompensation(110, new TimeSpan(16, 30, 0), new TimeSpan(17, 30, 0));

            var compensationCalculator = new CompensationCalculator(dayCompensation, dayEveningCompensation);
            var result = compensationCalculator.Execute(totalPayOfEmployees, timeSheetOfEmployees);

            Assert.True(result.Sum(x => x.TotalCompensation) == 0);
        }

        class GetNullTimeSheetOfEmployeesQueryMock : Mock<IGetTimeSheetOfEmployeesQuery>
        {
            public GetNullTimeSheetOfEmployeesQueryMock Execute()
            {
                Setup(x => x.Execute())
                    .Returns(Task.FromResult<TimeSheetOfEmployees>(new TimeSheetOfEmployees(DateTime.Parse("01.10.2017"), DateTime.Parse("31.10.2017"), new List<EmployeeTimeSheet>())));

                return this;
            }
        }

        class GetTimeSheetWithOtherEmployeesQueryMock : Mock<IGetTimeSheetOfEmployeesQuery>
        {
            public GetTimeSheetWithOtherEmployeesQueryMock Execute()
            {
                var employee = new EmployeeFromTimeSheet(999, "Фамилия И. О., Программист");

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

        class GetTotalPayOfEmployeesQueryMock : Mock<IGetTotalPayOfEmployeesQuery>
        {
            public GetTotalPayOfEmployeesQueryMock Execute()
            {
                var employee = new Employee(777, "Фамилия Имя Отчество", "Отдел №1");

                var payments = new List<Payment>()
                {
                    new Payment(new DateTime(2017, 10, 2, 12, 39, 0), 78, 0, 78),
                };

                var employeePayments = new EmployeePayments(employee, payments);
                var totalPayOfEmployees = new TotalPayOfEmployees(DateTime.Parse("01.10.2017"), DateTime.Parse("31.10.2017"), new List<EmployeePayments>() { employeePayments });

                Setup(x => x.Execute())
                    .Returns(Task.FromResult(totalPayOfEmployees));

                return this;
            }
        }
    }
}