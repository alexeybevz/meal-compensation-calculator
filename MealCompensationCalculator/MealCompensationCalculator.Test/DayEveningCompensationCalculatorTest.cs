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
    public class DayEveningCompensationCalculatorTest
    {
        [Fact]
        public void CalcDayEveningCompensationTest()
        {
            var getTotalPayOfEmployeesQueryMock = new GetTotalPayOfEmployeesQueryMock().Execute().Object;
            var totalPayOfEmployees = getTotalPayOfEmployeesQueryMock.Execute().Result;

            var getTimeSheetOfEmployeesQueryMock = new GetTimeSheetOfEmployeesQueryMock().Execute().Object;
            var timeSheetOfEmployees = getTimeSheetOfEmployeesQueryMock.Execute().Result;

            var dayCompensation = new MealCompensation(70, new TimeSpan(11, 0, 0), new TimeSpan(14, 0, 0));
            var dayEveningCompensation = new MealCompensation(110, new TimeSpan(16, 30, 0), new TimeSpan(17, 30, 0));

            var compensationCalculator = new CompensationCalculator(dayCompensation, dayEveningCompensation);
            var result = compensationCalculator.Execute(totalPayOfEmployees, timeSheetOfEmployees);

            Assert.True(result.Sum(x => x.TotalCompensation) == 1381);

            var expectedDaysWithZeroCompensation = new List<int> { 23, 27 };
            var resultDaysWithZeroCompensation = result.SelectMany(x => x.CompensationByDays).Where(x => x.Value.Compensation == 0).Select(x => x.Key).ToList();

            Assert.True(!resultDaysWithZeroCompensation.Except(expectedDaysWithZeroCompensation).Any() && resultDaysWithZeroCompensation.Count == 2);
        }
    }

    class GetTotalPayOfEmployeesQueryMock : Mock<IGetTotalPayOfEmployeesQuery>
    {
        public GetTotalPayOfEmployeesQueryMock Execute()
        {
            var employee = new Employee(777, "Фамилия Имя Отчество", "Отдел №1");

            var payments = new List<Payment>()
            {
                new Payment(new DateTime(2017, 10, 1, 11, 35, 0), 82, 0, 82),
                new Payment(new DateTime(2017, 10, 4, 11, 38, 0), 101, 0, 101),
                new Payment(new DateTime(2017, 10, 5, 11, 34, 0), 111, 0, 111),
                new Payment(new DateTime(2017, 10, 6, 11, 31, 0), 76, 0, 76),
                new Payment(new DateTime(2017, 10, 10, 11, 33, 0), 55, 0, 55),
                new Payment(new DateTime(2017, 10, 10, 17, 04, 0), 53, 0, 53),
                new Payment(new DateTime(2017, 10, 12, 11, 35, 0), 90, 0, 90),
                new Payment(new DateTime(2017, 10, 12, 17, 0, 0), 73, 0, 73),
                new Payment(new DateTime(2017, 10, 13, 11, 40, 0), 114, 0, 114),
                new Payment(new DateTime(2017, 10, 13, 17, 05, 0), 128, 0, 128),
                new Payment(new DateTime(2017, 10, 16, 11, 33, 0), 81, 0, 81),
                new Payment(new DateTime(2017, 10, 17, 11, 39, 0), 79, 0, 79),
                new Payment(new DateTime(2017, 10, 17, 17, 0, 0), 74, 0, 74),
                new Payment(new DateTime(2017, 10, 18, 17, 02, 0), 14, 0, 14),
                new Payment(new DateTime(2017, 10, 20, 11, 34, 0), 99, 0, 99),
                new Payment(new DateTime(2017, 10, 20, 17, 11, 0), 41, 0, 41),
                new Payment(new DateTime(2017, 10, 23, 11, 39, 0), 71, 0, 71),
                new Payment(new DateTime(2017, 10, 24, 11, 34, 0), 85, 0, 85),
                new Payment(new DateTime(2017, 10, 24, 17, 06, 0), 87, 0, 87),
                new Payment(new DateTime(2017, 10, 25, 11, 38, 0), 132, 0, 132),
                new Payment(new DateTime(2017, 10, 25, 17, 09, 0), 22, 0, 22),
                new Payment(new DateTime(2017, 10, 27, 11, 37, 0), 94, 0, 94),
                new Payment(new DateTime(2017, 10, 27, 17, 05, 0), 80, 0, 80),
                new Payment(new DateTime(2017, 10, 29, 11, 38, 0), 161, 0, 161),
                new Payment(new DateTime(2017, 10, 31, 11, 39, 0), 39, 0, 39),
            };

            var employeePayments = new EmployeePayments(employee, payments);
            var totalPayOfEmployees = new TotalPayOfEmployees(DateTime.Parse("01.10.2017"), DateTime.Parse("31.10.2017"), new List<EmployeePayments>() { employeePayments });

            Setup(x => x.Execute())
                .Returns(Task.FromResult(totalPayOfEmployees));

            return this;
        }
    }

    class GetTimeSheetOfEmployeesQueryMock : Mock<IGetTimeSheetOfEmployeesQuery>
    {
        public GetTimeSheetOfEmployeesQueryMock Execute()
        {
            var employee = new EmployeeFromTimeSheet(777, "Фамилия И. О., Программист");

            var timeSheetDays = new List<TimeSheetDay>()
            {
                new TimeSheetDay(1, "Я/ВЧ", "11/2"),
                new TimeSheetDay(2, "В", ""),
                new TimeSheetDay(3, "В", "В"),
                new TimeSheetDay(4, "Я/ВЧ", "11/2"),
                new TimeSheetDay(5, "Я/ВЧ", "11/2"),
                new TimeSheetDay(6, "РВ/ВЧ", "11/2"),
                new TimeSheetDay(7, "В", ""),
                new TimeSheetDay(8, "Я/ВЧ", "11/2"),
                new TimeSheetDay(9, "Я/ВЧ", "11/2"),
                new TimeSheetDay(10, "РВ/ВЧ", "11/2"),
                new TimeSheetDay(11, "В", ""),
                new TimeSheetDay(12, "Я/ВЧ", "11/2"),
                new TimeSheetDay(13, "Я/ВЧ", "11/2"),
                new TimeSheetDay(14, "В", ""),
                new TimeSheetDay(15, "В", ""),
                new TimeSheetDay(16, "Я/ВЧ", "11/2"),
                new TimeSheetDay(17, "Я/ВЧ", "11/2"),
                new TimeSheetDay(18, "РВ/ВЧ", "11/2"),
                new TimeSheetDay(19, "В", ""),
                new TimeSheetDay(20, "Я/ВЧ", "11/2"),
                new TimeSheetDay(21, "Я/ВЧ", "11/2"),
                new TimeSheetDay(22, "В", ""),
                new TimeSheetDay(23, "В", ""),
                new TimeSheetDay(24, "Я/ВЧ", "11/2"),
                new TimeSheetDay(25, "Я/ВЧ", "11/2"),
                new TimeSheetDay(26, "В", ""),
                new TimeSheetDay(27, "В", ""),
                new TimeSheetDay(28, "Я/ВЧ", "11/2"),
                new TimeSheetDay(29, "Я/ВЧ", "11/2"),
                new TimeSheetDay(30, "В", ""),
                new TimeSheetDay(31, "РВ/ВЧ", "11/2"),
            };

            var employeeTimeSheet = new List<EmployeeTimeSheet>() { new EmployeeTimeSheet(employee, timeSheetDays.ToDictionary(x => x.Day)) };
            var timeSheetOfEmployees = new TimeSheetOfEmployees(DateTime.Parse("01.10.2017"), DateTime.Parse("31.10.2017"), employeeTimeSheet);

            Setup(x => x.Execute())
                .Returns(Task.FromResult(timeSheetOfEmployees));

            return this;
        }
    }
}