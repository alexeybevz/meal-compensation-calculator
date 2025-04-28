using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MealCompensationCalculator.Domain.Domain.Models;
using MealCompensationCalculator.Domain.Domain.Queries;
using MealCompensationCalculator.Domain.Services;
using Moq;
using Xunit;

namespace MealCompensationCalculator.Test
{
    public class DayCompensationCalculatorForEmployeeWithTwoProfessionTest
    {
        [Fact]
        public void CalcDayCompensationForEmployeeWithTwoProfessionTest()
        {
            var getTotalPayOfEmployeesQueryMock = new GetTotalPayOfEmployeesQueryMock().Execute().Object;
            var totalPayOfEmployees = getTotalPayOfEmployeesQueryMock.Execute().Result;

            var getTimeSheetOfEmployeesQueryMock = new GetTimeSheetOfEmployeesQueryMock().Execute().Object;
            var timeSheetOfEmployees = getTimeSheetOfEmployeesQueryMock.Execute().Result;

            var dayCompensation = new MealCompensation(70, new TimeSpan(11, 0, 0), new TimeSpan(14, 0, 0));
            var dayEveningCompensation = new MealCompensation(110, new TimeSpan(16, 30, 0), new TimeSpan(17, 30, 0));

            var compensationCalculator = new CompensationCalculator(dayCompensation, dayEveningCompensation);
            var result = compensationCalculator.Execute(totalPayOfEmployees, timeSheetOfEmployees);

            Assert.True(result.Sum(x => x.TotalCompensation) == 833);

            var expectedDaysWithZeroCompensation = new List<int> { 18, 19, 20 };
            var resultDaysWithZeroCompensation = result.SelectMany(x => x.CompensationByDays).Where(x => x.Value == 0).Select(x => x.Key).ToList();

            Assert.True(!resultDaysWithZeroCompensation.Except(expectedDaysWithZeroCompensation).Any());
        }

        class GetTimeSheetOfEmployeesQueryMock : Mock<IGetTimeSheetOfEmployeesQuery>
        {
            public GetTimeSheetOfEmployeesQueryMock Execute()
            {
                var employee1 = new EmployeeFromTimeSheet(777, "Фамилия И. О., Программист");

                var timeSheetDays1 = new List<TimeSheetDay>()
                {
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

                var employee2 = new EmployeeFromTimeSheet(777, "Фамилия И. О., Техник-программист");

                var timeSheetDays2 = new List<TimeSheetDay>()
                {
                    new TimeSheetDay(1, "В", ""),
                    new TimeSheetDay(2, "Я", "8"),
                    new TimeSheetDay(3, "Я", "8"),
                };

                var employeeTimeSheet1 = new EmployeeTimeSheet(employee1, timeSheetDays1.ToDictionary(x => x.Day));
                var employeeTimeSheet2 = new EmployeeTimeSheet(employee2, timeSheetDays2.ToDictionary(x => x.Day));

                var employeeTimeSheets = new List<EmployeeTimeSheet>() { employeeTimeSheet1, employeeTimeSheet2 };
                var timeSheetOfEmployees = new TimeSheetOfEmployees(employeeTimeSheets);

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
                    new Payment(new DateTime(2017, 10, 3, 12, 39, 0), 88, 0, 88),
                    new Payment(new DateTime(2017, 10, 4, 12, 42, 0),103, 0, 103),
                    new Payment(new DateTime(2017, 10, 5, 12, 45, 0), 70, 0, 70),
                    new Payment(new DateTime(2017, 10, 6, 12, 41, 0), 100, 0, 100),
                    new Payment(new DateTime(2017, 10, 18, 12, 48, 0), 79, 0, 79),
                    new Payment(new DateTime(2017, 10, 19, 12, 42, 0), 90, 0, 90),
                    new Payment(new DateTime(2017, 10, 20, 12, 45, 0), 83, 0, 83),
                    new Payment(new DateTime(2017, 10, 23, 12, 44, 0), 76, 0, 76),
                    new Payment(new DateTime(2017, 10, 24, 12, 44, 0), 63, 0, 63),
                    new Payment(new DateTime(2017, 10, 25, 12, 40, 0), 97, 0, 97),
                    new Payment(new DateTime(2017, 10, 26, 12, 39, 0), 99, 0, 99),
                    new Payment(new DateTime(2017, 10, 27, 12, 42, 0), 92, 0, 92),
                    new Payment(new DateTime(2017, 10, 30, 12, 38, 0), 70, 0, 70),
                    new Payment(new DateTime(2017, 10, 31, 12, 40, 0), 75, 0, 75),
                };

                var employeePayments = new EmployeePayments(employee, payments);
                var totalPayOfEmployees = new TotalPayOfEmployees(new List<EmployeePayments>() { employeePayments });

                Setup(x => x.Execute())
                    .Returns(Task.FromResult(totalPayOfEmployees));

                return this;
            }
        }
    }
}